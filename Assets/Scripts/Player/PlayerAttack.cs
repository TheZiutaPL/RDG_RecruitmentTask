using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private const string ATTACK_ANIMATION_TRIGGER = "attack";

    private bool canAttack = true;
    public bool ToggleAttack(bool toggle) => canAttack = toggle;

    [SerializeField] private Animator animator;
    [SerializeField] private float attackCooldown;
    private float cooldownTimer;
    private void SetAttackCooldown() => cooldownTimer = attackCooldown;
    private bool IsOnCooldown() => cooldownTimer > 0;

    private void Start()
    {
        PlayerEntity.Instance.OnRespawn += () => ToggleAttack(true);
    }

    private void Update()
    {
        if (cooldownTimer > 0)
            cooldownTimer -= Time.deltaTime;
    }

    public void HandleAttack()
    {
        if (!canAttack || IsOnCooldown())
            return;

        SetAttackCooldown();
        animator.SetTrigger(ATTACK_ANIMATION_TRIGGER);
    }

    private void OnEnable()
    {
        InputManager.GameInputs.Game.Attack.performed += _ => HandleAttack();
    }

    private void OnDisable()
    {
        InputManager.GameInputs.Game.Attack.performed -= _ => HandleAttack();
    }
}
