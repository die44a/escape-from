using System;
using _Project.Runtime.Core.General;
using UnityEngine;

namespace _Project.Runtime.Player.Controllers
{
    public class PlayerMovementController : MovementController, IDashProvider
    {
        [SerializeField] private float dashForce = 12f;
        [SerializeField] private float dashCooldown = 1.5f;

        public float DashProgress => Mathf.Clamp01(_timeElapsed / dashCooldown);
        public float RemainingDashProgress => dashCooldown - _timeElapsed;
        public bool IsDashReady => _timeElapsed >= dashCooldown ;
        public event Action OnDashFailed;

        private float _timeElapsed;
        private IDashProvider _dashProviderImplementation;

        private void Start()
        {
            _timeElapsed = dashCooldown;
        }
        
        private void Update()
        {
            if (_timeElapsed < dashCooldown) 
                _timeElapsed += Time.deltaTime;
        }
        
        public void NotifyDashFailed()
        {
            OnDashFailed?.Invoke();
        }
        
        public void Dash(Vector2 direction)
        {
            if (_timeElapsed < dashCooldown)
                return;
            
            _timeElapsed = 0f;
            
            if (IsKnockedBack) return;
            Rb.linearVelocity = direction.normalized * dashForce;
        }
        
        public void ResetMovement()
        {
            Rb.linearVelocity = Vector3.zero;
            Rb.angularVelocity = 0f;
        }
    }
}