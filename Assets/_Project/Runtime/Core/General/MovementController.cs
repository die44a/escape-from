using System.Collections;
using UnityEngine;

namespace _Project.Runtime.Core.General
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class MovementController : MonoBehaviour
    {
        [SerializeField] protected float moveSpeed = 6f;
        [SerializeField] protected float knockbackDamping = 5f;
        
        protected Rigidbody2D Rb;
        protected bool IsKnockedBack;
        private float _originalDamping;
        private Coroutine _knockbackCoroutine;
        
        public Vector2 LastDirection { get; protected set; } = Vector2.down;
        public Vector2 CurrentVelocity => Rb ? Rb.linearVelocity : Vector2.zero;
        public bool IsMoving => CurrentVelocity.sqrMagnitude > 0.01;

        protected virtual void Awake()
        {
            Rb = GetComponent<Rigidbody2D>();
            _originalDamping = Rb.linearDamping;
        }

        public virtual void ApplyMovement(Vector2 direction)
        {
            if (IsKnockedBack) return;

            var velocity = direction.sqrMagnitude > 1f ? direction.normalized : direction;
            Rb.linearVelocity = velocity * moveSpeed;
            UpdateDirection(direction);
        }

        private void UpdateDirection(Vector2 direction)
        {
            if (direction.sqrMagnitude > 0.01)
                LastDirection = direction.normalized;
            else if (CurrentVelocity.sqrMagnitude > 0.01)
                LastDirection = CurrentVelocity.normalized;
        }

        public virtual void ApplyKnockback(Vector2 force, float duration)
        {
            if (_knockbackCoroutine != null) StopCoroutine(_knockbackCoroutine);
            _knockbackCoroutine = StartCoroutine(KnockbackRoutine(force, duration));
        }

        private IEnumerator KnockbackRoutine(Vector2 force, float duration)
        {
            IsKnockedBack = true;
            Rb.linearDamping = knockbackDamping;
            Rb.linearVelocity = Vector2.zero;

            Rb.AddForce(force, ForceMode2D.Impulse);

            yield return new WaitForSeconds(duration);

            Rb.linearDamping = _originalDamping;
            IsKnockedBack = false;
        }

        public void Stop() => Rb.linearVelocity = Vector2.zero;

        public void StopPhysics()
        {
            Rb.linearVelocity = Vector2.zero;
            Rb.angularVelocity = 0f;
            Rb.simulated = false;
        }
        
        public bool GetKnockbackStatus() => IsKnockedBack;
    }
}