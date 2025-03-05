using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    private async void HandleAttack()
    {
        if (!canAttack || ai.IsBehaviorOverriden())
            return;

        canAttack = false;
        
        await AttackTask();

        canAttack = true;
    }

    private async Task AttackTask()
    {
        ai.SetOverrideBehavior(true);

        //Sets animation
        animator.SetTrigger(ATTACK_ANIMATION_KEY);
        await Task.Delay((int)(attackBehaviorOverrideTime * 1000));

        ai.SetOverrideBehavior(false);
    }
}
