using UnityEngine;

namespace _Project.Runtime.Enemy
{
    public class RangedEnemy : EnemyController
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform firePoint;

        protected override void TryAttack()
        {
            if (Time.time - LastAttackTime < attackCooldown) return;

            FireProjectile();
            
            LastAttackTime = Time.time;
        }

        private void FireProjectile()
        {
            if (!projectilePrefab) return;

            var proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            
            var direction = (TargetPlayerPosition - (Vector2)firePoint.position).normalized;
            proj.transform.right = direction;
        }
    }
}