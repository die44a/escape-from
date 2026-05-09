using _Project.Runtime.Core.General;
using UnityEngine;

namespace _Project.Runtime.Enemy
{
    public class MeleeEnemy : EnemyController
    {
        [SerializeField] protected float damageAmount = 5f;
        [SerializeField] protected float knockbackForce = 5f;
        [SerializeField] protected float knockbackDuration = 0.3f;
        [SerializeField] protected float attackRange = 0.7f;
        [SerializeField] protected float offsetDistance = 0.5f;
        private Vector2 AttackOffset => _movement.LastDirection * offsetDistance;
        
        protected override void TryAttack()
        {
            if (Time.time - LastAttackTime < attackCooldown) return;
            if (!Physics2D.OverlapCircle(transform.position, attackRange, playerMask))
                return;
            LastAttackTime = Time.time;
            IsAttacking = true;
        }
        
        public void OnHitFrame()
        {
            var hit = Physics2D.OverlapCircle(transform.position + (Vector3)AttackOffset, attackRange, playerMask);
            if (!hit) return;
            if (!hit.TryGetComponent<IDamageable>(out var damageable)) return;
            damageable.ApplyDamage(damageAmount);
            if (hit.TryGetComponent<MovementController>(out var pMovement))
            {
                var dir = (hit.transform.position - transform.position).normalized;
                pMovement.ApplyKnockback(dir * knockbackForce, knockbackDuration);
            }
        }
        
        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            var center = Application.isPlaying ? (Vector3)_startPosition : transform.position;
            Gizmos.DrawWireSphere(center, leashRadius > 0f ? leashRadius : detectionRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + (Vector3)AttackOffset, attackRange);

            if (Player != null && CanSeePlayer())
            {
                Gizmos.DrawLine(transform.position, TargetPlayerPosition);
            }
        }
    }
}