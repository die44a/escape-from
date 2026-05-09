using _Project.Runtime.Core.General;
using UnityEngine;

namespace _Project.Runtime.Enemy
{
    public class EnemyMovement : MovementController 
    {
        [SerializeField] private bool useNoFrictionMaterial = true;

        private static PhysicsMaterial2D _sharedNoFrictionMaterial;

        protected override void Awake()
        {
            base.Awake();

            if (useNoFrictionMaterial)
                ApplyNoFrictionToColliders();

            if (Rb)
                Rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        private void ApplyNoFrictionToColliders()
        {
            if (_sharedNoFrictionMaterial == null)
            {
                _sharedNoFrictionMaterial = new PhysicsMaterial2D("Enemy_NoFriction")
                {
                    friction = 0f,
                    bounciness = 0f
                };
            }

            var colliders = GetComponents<Collider2D>();
            foreach (var t in colliders)
                t.sharedMaterial = _sharedNoFrictionMaterial;
        }

        public void MoveTowards(Vector2 target)
        {
            var direction = (target - (Vector2)transform.position).normalized;
            ApplyMovement(direction);
        }
    }
}