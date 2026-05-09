using _Project.Runtime.Core.General;
using UnityEngine;

namespace _Project.Runtime.Enemy
{
    public class MeleeEnemy : EnemyController
    {
        [SerializeField] protected float damageAmount = 5f;
        [SerializeField] protected float knockbackForce = 5f;

        protected override void TryAttack()
        {
            if (Time.time - LastAttackTime < attackCooldown) return;

            var hit = Physics2D.OverlapCircle(transform.position, attackRange, playerMask);

            if (!hit) return;
            if (!hit.TryGetComponent<IDamageable>(out var damageable)) return;
            damageable.ApplyDamage(damageAmount);
            LastAttackTime = Time.time;

            if (!hit.TryGetComponent<MovementController>(out var pMovement)) return;
            Vector2 dir = (hit.transform.position - transform.position).normalized;
            pMovement.ApplyKnockback(dir * knockbackForce, 0.2f);
        }
    }
}