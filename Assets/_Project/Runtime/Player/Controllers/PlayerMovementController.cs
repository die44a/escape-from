using UnityEngine;

namespace _Project.Runtime.Player.Controllers
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float dashForce = 12f;

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void ApplyMovement(Vector2 input)
        {
            var direction = input.sqrMagnitude > 1f ? input.normalized : input;
            _rb.linearVelocity = direction * moveSpeed;
        }

        public void Dash(Vector2 direction)
        {
            _rb.linearVelocity = direction.normalized * dashForce;
        }

        public void Stop()
        {
            _rb.linearVelocity = Vector2.zero;
        }
    }
}