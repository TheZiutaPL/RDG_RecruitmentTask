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

    [Header("Guide Spell")]
    [SerializeField] private GuideSpell guideSpellPrefab;

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
        Vector3 targetPosition = GameManager.Instance.GetClosestPlayerGoalPosition(transform.position);

        //Spawns projectile at player position
        GuideSpell projectile = Instantiate(guideSpellPrefab, transform.position + guideSpawnDistance * (targetPosition - transform.position).normalized, Quaternion.identity);

        //Sets projectile values and cooldown
        projectile.SetGuideSpell(targetPosition);
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
