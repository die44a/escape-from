using UnityEngine;

namespace _Project.Runtime.Core.Weapon
{
    public class MeleeWeapon : WeaponBase
    {
        private MeleeWeaponConfig _weaponConfig;

        public override void TryAttack()
        {
            if (Time.time < NextAttackTime) return;
            
            NextAttackTime = Time.time + _weaponConfig.attackRate;
            Animator.SetTrigger(AttackTrigger);
        }
        
        public override void InitWeapon(WeaponConfig config)
        {
            base.InitWeapon(config);

            _weaponConfig = config as MeleeWeaponConfig;

            if (_weaponConfig == null)
                Debug.LogError($"На объект {gameObject.name} пришел неверный тип конфига!");
        }

        public override void OnAnimationAction()
        {
            var hitPos = visualChild.position; 
            var hitEnemies = Physics2D.OverlapCircleAll(hitPos, _weaponConfig.hitRadius, ~0);

            foreach (var hit in hitEnemies)
            {
                if (hit.gameObject == Player.gameObject)
                    continue;
                
                if (hit.TryGetComponent(out IDamageable damageable))
                    damageable.ApplyDamage(_weaponConfig.damage);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(visualChild.position, _weaponConfig.hitRadius);
        }
    }
}