using _Project.Runtime.Core.General;
using UnityEngine;

namespace _Project.Runtime.Core.Traps
{
    public class LogTrap : MonoBehaviour
    {
        [SerializeField] private float damageAmount = 5f;
        [SerializeField] private float knockbackForce = 100f;
        [SerializeField] private float knockbackDuration = 0.2f;

        [SerializeField] private bool applyKnockback = true;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var damageable = collision.gameObject.GetComponentInParent<IDamageable>();

            if (damageable == null) return;
            damageable.ApplyDamage(damageAmount);

            if (applyKnockback)
            {
                ApplyKnockbackEffect(collision);
            }
        }

        private void ApplyKnockbackEffect(Collision2D collision)
        {
            var controller = collision.gameObject.GetComponentInParent<MovementController>();

            if (controller == null) return;
            var direction = (collision.transform.position - transform.position).normalized;
            
            controller.ApplyKnockback(direction * knockbackForce, knockbackDuration);
        }
    }
}