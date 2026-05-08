using UnityEngine;

namespace _Project.Runtime.Enemy
{
    public class Orc : MeleeEnemy
    {
        [Header("Orc Specific")]
        [SerializeField] private float enrageHealthThreshold = 0.3f; // Ярость при 30% HP
        [SerializeField] private float speedMultiplierInEnrage = 1.5f;

        private EnemyDamageController _health;
        private bool _isEnraged;

        protected override void Awake()
        {
            base.Awake();
            _health = GetComponent<EnemyDamageController>();
        }

        // Мы можем переопределить атаку, чтобы добавить Орку спецэффектов
        protected override void TryAttack()
        {
            // Проверяем здоровье для ярости
            CheckEnrage();

            // Вызываем базовую мили-атаку (OverlapCircle)
            base.TryAttack();
        }

        private void CheckEnrage()
        {
            if (!_isEnraged && _health.HealthPercentage <= enrageHealthThreshold)
            {
                _isEnraged = true;
                // Тут можно дернуть аниматор или изменить скорость через _movement
                Debug.Log("Орк в ярости!");
            }
        }
    }
}