using UnityEngine;

namespace _Project.Runtime.Enemy
{
    public class RangedEnemy : EnemyController
    {
        [Header("Ranged Settings")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform firePoint;

        protected override void TryAttack()
        {
            if (Time.time - _lastAttackTime < attackCooldown) return;

            // Логика стрельбы
            FireProjectile();
            
            _lastAttackTime = Time.time;
        }

        private void FireProjectile()
        {
            if (projectilePrefab == null) return;

            GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            
            // Настройка направления снаряда (простая логика)
            Vector2 direction = (TargetPlayerPosition - (Vector2)firePoint.position).normalized;
            proj.transform.right = direction; // Поворачиваем снаряд в сторону игрока
        }
    }
}