using UnityEngine;

namespace _Project.Runtime.Core.Weapon
{
    public class MeleeWeapon : WeaponBase
    {
        [Header("Melee Specific")]
        [SerializeField] private MeleeWeaponConfig weaponConfig;
        [SerializeField] private LayerMask enemyLayer;

        public override void TryAttack()
        {
            if (Time.time < NextAttackTime) return;
            
            NextAttackTime = Time.time + weaponConfig.attackRate;
            Animator.SetTrigger(AttackTrigger);
        }

        public override void OnAnimationAction()
        {
            var hitPos = visualChild.position; 
            var hitEnemies = Physics2D.OverlapCircleAll(hitPos, weaponConfig.hitRadius, enemyLayer);

            foreach (var enemy in hitEnemies)
            {
                Debug.Log($"Попал по {enemy.name} и нанес {weaponConfig.damage} урона");
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(visualChild.position, weaponConfig.hitRadius);
        }
    }
}