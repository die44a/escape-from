using System;
using System.Collections;
using UnityEngine;

namespace _Project.Runtime.Player.Controllers
{
    public class HealthTimeController : MonoBehaviour, IHealthObservable, IDamageable
    {
        [SerializeField] private float maxHealthTime = 60f; 
        [SerializeField] private float invulnerabilityDuration = 0.7f;
        
        private float _currentHealthTime;
        private float _timeModifier;
        private bool _isInvulnerable = false;
        private bool _isDead = false;
        
        public event Action<float> OnHealthChanged; 
        public event Action OnDeath;
        public event Action OnHit; 

        public float CurrentHealth => _currentHealthTime;
        public float InvulnerabilityDuration => invulnerabilityDuration;

        private void Start()
        {
            _currentHealthTime = maxHealthTime;
            OnHealthChanged?.Invoke(_currentHealthTime);
        }

        private void Update()
        {
            if (_isDead) return;

            ReduceHealth(Time.deltaTime * _timeModifier);
        }

        public void ApplyDamage(float amount)
        {
            if (_isDead || _isInvulnerable) return;
            
            _currentHealthTime = Mathf.Max(_currentHealthTime - amount, 0);
            OnHealthChanged?.Invoke(_currentHealthTime);
            OnHit?.Invoke(); 

            if (_currentHealthTime <= 0)
                Die();
            else
                StartCoroutine(InvulnerabilityRoutine());
        }

        private IEnumerator InvulnerabilityRoutine()
        {
            _isInvulnerable = true;
            yield return new WaitForSeconds(invulnerabilityDuration);
            _isInvulnerable = false;
        }

        private void ReduceHealth(float amount)
        {
            _currentHealthTime -= amount;
            _currentHealthTime = Mathf.Max(_currentHealthTime, 0);
        
            OnHealthChanged?.Invoke(_currentHealthTime);

            if (_currentHealthTime <= 0 && !_isDead)
                Die();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void Die()
        {
            _isDead = true;
            Debug.Log("Игрок погиб (время истекло)");
            OnDeath?.Invoke();
        }
        
        public void SetTimeModifier(float multiplier)
            => _timeModifier = Mathf.Max(multiplier, 0); 

        public void AddTime(float amount)
        {
            if (_isDead || amount < 0) return;
            _currentHealthTime = Mathf.Min(_currentHealthTime + amount, maxHealthTime);
            OnHealthChanged?.Invoke(_currentHealthTime);
        }
    }
}
