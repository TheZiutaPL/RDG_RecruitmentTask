using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGuide : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private float guideSpawnDistance = .5f;
    [SerializeField] private float guideCooldown = 2.5f;
    private float cooldown;
    private bool IsOnCooldown() => cooldown > 0;
    private void SetCooldown() => cooldown = guideCooldown;

    [Header("Guide Spell Settings")]
    [SerializeField] private Projectile guideSpellPrefab;
    [SerializeField] private float guideSpeed = 3;
    [SerializeField] private float guideLifetime = 3.5f;

    private void Update()
    {
        if (IsOnCooldown())
            cooldown -= Time.deltaTime;
    }

    public void UseGuideSpell()
    {
        if (IsOnCooldown())
            return;

        //Gets goal direction
        Vector3 direction = (GameManager.Instance.GetClosestPlayerGoalPosition(transform.position) - transform.position).normalized;

        //Spawns projectile at player position
        Projectile projectile = Instantiate(guideSpellPrefab, transform.position + guideSpawnDistance * direction, Quaternion.identity);

        //Sets projectile values and cooldown
        projectile.SetProjectile(direction, guideSpeed, guideLifetime);
        SetCooldown();
    }

    private void OnEnable()
    {
        InputManager.GameInputs.Game.GuideSpell.performed += _ => UseGuideSpell();
    }

    private void OnDisable()
    {
        InputManager.GameInputs.Game.GuideSpell.performed -= _ => UseGuideSpell();
    }
}
