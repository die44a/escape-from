using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Zenject;

namespace _Project.Runtime.Enemy
{
    public class EnemyDamageController : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float invulnerabilityDuration = 0.2f;

        public UnityEvent<float, float> onHealthChanged;
        public UnityEvent onHit;
        public UnityEvent onDeath;

        private float _currentHealth;
        private bool _isDead;

        private void Awake()
        {
            _currentHealth = maxHealth;
        }

        public void ApplyDamage(float amount)
        {
            if (_isDead) return;
            
            _currentHealth = Mathf.Max(_currentHealth - amount, 0);
            
            onHealthChanged?.Invoke(_currentHealth, maxHealth);
            onHit?.Invoke(); 

            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (_isDead) return;
            _isDead = true;
            
            onDeath?.Invoke();
            if (TryGetComponent<Collider2D>(out var col)) col.enabled = false;
            
            // Здесь можно запустить анимацию смерти или просто удалить объект
            // Destroy(gameObject, 1f); 
        }
        

        public bool IsDead => _isDead;
        public float HealthPercentage => _currentHealth / maxHealth;
    }
}