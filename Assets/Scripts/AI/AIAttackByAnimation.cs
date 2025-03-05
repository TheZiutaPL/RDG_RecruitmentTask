using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FollowAI))]
public class AIAttackByAnimation : MonoBehaviour
{
    private FollowAI ai;
    [SerializeField] private Animator animator;
    [SerializeField] private float attackBehaviorOverrideTime;
    private bool canAttack = true;

    private const string ATTACK_ANIMATION_KEY = "attack";

    private void Awake()
    {
        ai = GetComponent<FollowAI>();

        ai.OnChaseTargetReached += HandleAttack;
    }

    private void HandleAttack()
    {
        if (!canAttack || ai.IsBehaviorOverriden())
            return;

        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        canAttack = false;
        ai.SetOverrideBehavior(true);

        //Sets animation
        animator.SetTrigger(ATTACK_ANIMATION_KEY);
        yield return new WaitForSeconds(attackBehaviorOverrideTime);

        ai.SetOverrideBehavior(false);
        canAttack = true;
    }
}
