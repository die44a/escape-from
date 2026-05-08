using _Project.Runtime.Core.General;
using UnityEngine;

namespace _Project.Runtime.Enemy
{
    public class MeleeEnemy : EnemyController
    {
        [Header("Melee Settings")]
        [SerializeField] protected float damageAmount = 10f;
        [SerializeField] protected float knockbackForce = 5f;

        protected override void TryAttack()
        {
            if (Time.time - _lastAttackTime < attackCooldown) return;

            // Логика OverlapCircle
            Collider2D hit = Physics2D.OverlapCircle(transform.position, attackRange, playerMask);

            if (hit != null)
            {
                if (hit.TryGetComponent<IDamageable>(out var damageable))
                {
                    damageable.ApplyDamage(damageAmount);
                    _lastAttackTime = Time.time;

                    if (hit.TryGetComponent<MovementController>(out var pMovement))
                    {
                        Vector2 dir = (hit.transform.position - transform.position).normalized;
                        pMovement.ApplyKnockback(dir * knockbackForce, 0.2f);
                    }
                }
            }
        }
    }
}