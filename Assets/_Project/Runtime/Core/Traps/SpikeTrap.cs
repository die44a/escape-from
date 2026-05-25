using _Project.Runtime.Core.General;
using UnityEngine;

namespace _Project.Runtime.Core.Traps
{
    public class SpikeTrap : BaseTrap
    {
        [SerializeField] private float knockbackForce = 2f;
        [SerializeField] private float knockbackDuration = 0.15f;
        [SerializeField] private bool applyKnockback = true;

        public void OnSpikesActionDamage()
        {
            DealDamageToAll();
            ApplyKnockbackToAll();
        }

        private void ApplyKnockbackToAll()
        {
            if (!applyKnockback)
                return;

            if (TargetsInRange.Count == 0)
                return;

            var trapPosition = (Vector2)transform.position;

            foreach (var target in TargetsInRange)
            {
                if (target == null)
                    continue;

                var component = target as Component;
                if (!component)
                    continue;

                var controller = component.GetComponentInParent<MovementController>();
                if (controller == null)
                    continue;

                var direction = ((Vector2)component.transform.position - trapPosition).normalized;

                controller.ApplyKnockback(direction * knockbackForce, knockbackDuration);
            }
        }
    }
}