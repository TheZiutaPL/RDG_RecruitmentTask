using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class EntityAnimationHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private const string HIT_ANIMATION_KEY = "hit";
    private const string DEATH_ANIMATION_KEY = "dead";

    private void Awake()
    {
        Entity entity = GetComponent<Entity>();

        entity.OnDamage += SetHitAnimation;
        entity.OnDeath += SetDeathAnimation;
    }

    private void SetHitAnimation() => animator.SetTrigger(HIT_ANIMATION_KEY);
    private void SetDeathAnimation() => animator.SetTrigger(DEATH_ANIMATION_KEY);
}
