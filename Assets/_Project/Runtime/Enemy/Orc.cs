using UnityEngine;

namespace _Project.Runtime.Enemy
{
    public class Orc : MeleeEnemy
    {
        [SerializeField] private float enrageHealthThreshold = 0.3f;
        [SerializeField] private float speedMultiplierInEnrage = 1.5f;

        private EnemyDamageController _health;
        private bool _isEnraged;

        protected override void Awake()
        {
            base.Awake();
            _health = GetComponent<EnemyDamageController>();
        }

        protected override void TryAttack()
        {
            CheckEnrage();

            base.TryAttack();
        }

        private void CheckEnrage()
        {
            if (_isEnraged || !(_health.HealthPercentage <= enrageHealthThreshold)) return;
            _isEnraged = true;
        }
    }
}