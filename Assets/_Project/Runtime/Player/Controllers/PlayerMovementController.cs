using System;
using _Project.Runtime.Core.General;
using UnityEngine;

namespace _Project.Runtime.Player.Controllers
{
    public class PlayerMovementController : MovementController, IDashProvider
    {
        [SerializeField] private float dashForce = 12f;
        [SerializeField] private float dashCooldown = 1.5f;

        public float DashProgress => dashCooldown > 0 ? Mathf.Clamp01(_timeElapsed / dashCooldown) : 1f;
        public float RemainingDashProgress => dashCooldown - _timeElapsed;
        public bool IsDashReady => _timeElapsed >= dashCooldown ;
        
        private float _timeElapsed;
        
        private void Start()
        {
            _timeElapsed = dashCooldown;
        }
        
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