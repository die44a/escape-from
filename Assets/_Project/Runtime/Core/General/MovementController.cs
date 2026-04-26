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

        protected virtual void Awake()
        {
            Rb = GetComponent<Rigidbody2D>();
            _originalDamping = Rb.linearDamping;
        }

        public virtual void ApplyMovement(Vector2 direction)
        {
            if (IsKnockedBack) return;

            Vector2 velocity = direction.sqrMagnitude > 1f ? direction.normalized : direction;
            Rb.linearVelocity = velocity * moveSpeed;
        }

        public virtual void ApplyKnockback(Vector2 force, float duration)
        {
            StopAllCoroutines();
            StartCoroutine(KnockbackRoutine(force, duration));
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
        
        public bool GetKnockbackStatus() => IsKnockedBack;
    }
}