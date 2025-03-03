using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private bool belongsToPlayer;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius;
    [SerializeField] private LayerMask attackMask;
    private Collider2D[] cols = new Collider2D[5];

    public void Attack()
    {
        int overlapped = Physics2D.OverlapCircleNonAlloc(attackPoint.position, attackRadius, cols);
        for (int i = 0; i < overlapped; i++)
        {
            //Gets Entity instance from collider and checks if hit entity should be damaged by this entity
            if (cols[i].TryGetComponent(out Entity entity) && (belongsToPlayer != (entity == PlayerEntity.Instance)))
                entity.Damage();
        }
    }

    private void OnDrawGizmos()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
