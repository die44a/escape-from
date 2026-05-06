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

            if (!_weaponConfig)
                Debug.LogError($"На объект {gameObject.name} пришел неверный тип конфига!");
        }

        public override void OnAnimationAction()
        {
            if (!_weaponConfig) return;
            
            Collider2D[] hitEnemies;
            Vector2 hitPos = visualChild.position;
            
            var currentAngle = visualChild.rotation.eulerAngles.z + 45f;
            
            if (_weaponConfig.shape == AttackShape.Box)
                hitEnemies = Physics2D.OverlapBoxAll(hitPos, _weaponConfig.boxSize, currentAngle, ~0);
            else
                hitEnemies = Physics2D.OverlapCircleAll(hitPos, _weaponConfig.hitRadius, ~0);
            
            foreach (var hit in hitEnemies)
            {
                if (hit.gameObject == Player.gameObject)
                    continue;
                
                var wallHit = Physics2D.Linecast(transform.position, hit.transform.position, obstacleLayersMask);
                
                if (wallHit.collider)
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
            if (!visualChild || _weaponConfig == null) return;
    
            Gizmos.color = Color.red;
            var oldMatrix = Gizmos.matrix;
            
            var offset = Quaternion.Euler(0, 0, 45f);
            Gizmos.matrix = Matrix4x4.TRS(visualChild.position, visualChild.rotation * offset, Vector3.one);

            if (_weaponConfig.shape == AttackShape.Box)
            {
                var finalSize = Vector2.Scale(_weaponConfig.boxSize, visualChild.lossyScale);
                Gizmos.DrawWireCube(Vector3.zero, finalSize);
            }
            else
            {
                var finalRadius = _weaponConfig.hitRadius * Mathf.Max(visualChild.lossyScale.x, visualChild.lossyScale.y);
                Gizmos.DrawWireSphere(Vector3.zero, finalRadius);
            }

            Gizmos.matrix = oldMatrix;
        }
    }
}