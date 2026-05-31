using System;
using UnityEngine;
using _Project.Services.Audio;
using Zenject;

namespace _Project.Runtime.Enemy
{
    public class EnemyDamageController : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHealth = 100f;

        public event Action<float, float> OnHealthChanged;
        public Action OnHit;
        public Action OnDeath;

        private float _currentHealth;
        private bool _isDead;
        
        [Inject] private IAudioService _audioService;

        private void Awake()
        {
            _currentHealth = maxHealth;
        }

        public void ApplyDamage(float amount)
        {
            if (_isDead) return;
            
            _currentHealth = Mathf.Max(_currentHealth - amount, 0);
            
            OnHealthChanged?.Invoke(_currentHealth, maxHealth);
            OnHit?.Invoke(); 

            _audioService?.PlaySound(SoundId.HitMelee);

            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (_isDead) return;
            _isDead = true;

            _audioService?.PlaySound(SoundId.DeathMelee);
            
            OnDeath?.Invoke();
            
            if (TryGetComponent<Collider2D>(out var col)) 
                col.enabled = false;
        }


        public bool IsDead => _isDead;
        public float HealthPercentage => _currentHealth / maxHealth;
    }
}