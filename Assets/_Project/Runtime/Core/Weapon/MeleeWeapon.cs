using UnityEngine;

namespace _Project.Runtime.Core.Weapon
{
    public class MeleeWeapon : WeaponBase
    {
        [Header("Melee Specific")]
        [SerializeField] private MeleeWeaponData weaponData;
        [SerializeField] private float hitRadius = 1.5f;
        [SerializeField] private LayerMask enemyLayer;

        public override void TryAttack()
        {
            if (Time.time < NextAttackTime) return;
            
            NextAttackTime = Time.time + weaponData.attackRate;
            Animator.SetTrigger(AttackTrigger);
        }

        public override void OnAnimationAction()
        {
            var hitPos = visualChild.position; 
            var hitEnemies = Physics2D.OverlapCircleAll(hitPos, hitRadius, enemyLayer);

            foreach (var enemy in hitEnemies)
            {
                Debug.Log($"Попал по {enemy.name} и нанес {weaponData.damage} урона");
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(visualChild.position, hitRadius);
        }
    }
}