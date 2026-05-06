using _Project.Runtime.Core.General;
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
            Collider2D[] hitEnemies;
            Vector2 hitPos = visualChild.position;
            
            if (_weaponConfig.shape == AttackShape.Box)
            {
                var angle = visualChild.eulerAngles.z;
                hitEnemies = Physics2D.OverlapBoxAll(hitPos, _weaponConfig.boxSize, angle, ~0);
            }
            else
                hitEnemies = Physics2D.OverlapCircleAll(hitPos, _weaponConfig.hitRadius, ~0);
            
            foreach (var hit in hitEnemies)
            {
                if (hit.gameObject == Player.gameObject)
                    continue;
                
                if (hit.TryGetComponent(out IDamageable damageable))
                    damageable.ApplyDamage(_weaponConfig.damage);
                
                if (hit.TryGetComponent(out MovementController moveCtrl))
                {
                    Vector2 knockbackDir = (hit.transform.position - Player.transform.position).normalized;

                    moveCtrl.ApplyKnockback(knockbackDir * _weaponConfig.knockbackForce, _weaponConfig.knockbackDuration);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (!visualChild) return;
            
            Gizmos.color = Color.red;
            var oldMatrix = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(visualChild.position, visualChild.rotation, Vector3.one);
            var shape = _weaponConfig != null ? _weaponConfig.shape : AttackShape.Circle;
            if (shape == AttackShape.Box)
            {
                var size = _weaponConfig != null ? _weaponConfig.boxSize : new Vector2(1.2f, 0.6f);
                Gizmos.DrawWireCube(Vector3.zero, size);
            }
            else
            {
                var radius = _weaponConfig != null ? _weaponConfig.hitRadius : 1.2f;
                Gizmos.DrawWireSphere(Vector3.zero, radius);
            }
            Gizmos.matrix = oldMatrix;
        }
    }
}