using _Project.Runtime.Core.General;
using UnityEngine;

namespace _Project.Runtime.Player.Controllers
{
    public class PlayerMovementController : MovementController
    {
        [SerializeField] private float dashForce = 12f;

        public void Dash(Vector2 direction)
        {
            if (IsKnockedBack) return;
            Rb.linearVelocity = direction.normalized * dashForce;
        }
    }
}