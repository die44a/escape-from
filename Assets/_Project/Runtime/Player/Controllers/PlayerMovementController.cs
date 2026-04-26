using System;
using _Project.Runtime.Core.General;
using UnityEngine;

namespace _Project.Runtime.Player.Controllers
{
    public class PlayerMovementController : MovementController, IDashProvider
    {
        [SerializeField] private float dashForce = 12f;
        [SerializeField] private float dashCooldown = 1.5f;

        public float DashProgress => _timeElapsed / dashCooldown;
        public bool IsDashReady => _timeElapsed >= dashCooldown;
        
        private float _timeElapsed = 1.5f;

        private void Update()
        {
            if (_timeElapsed < dashCooldown) 
                _timeElapsed += Time.deltaTime;
        }
        
        public void Dash(Vector2 direction)
        {
            if (_timeElapsed < dashCooldown)
                return;
            
            _timeElapsed = 0f;
            
            if (IsKnockedBack) return;
            Rb.linearVelocity = direction.normalized * dashForce;
        }
    }
}