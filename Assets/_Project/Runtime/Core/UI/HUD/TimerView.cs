using TMPro;
using UnityEngine;
using Zenject;

namespace _Project.Runtime.Core.UI.HUD
{
    public class TimeView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textDisplay;
        [SerializeField] private float smoothSpeed = 15f;
        [SerializeField] private float hitFlashDuration = 0.2f;
        
        [Inject] private IHealthObservable _healthModel;

        private float _hitTimer;
        private float _targetTime;
        private float _displayedTime;

        private void Start()
        {
            _targetTime = _healthModel.CurrentHealth;
            _displayedTime = _targetTime;
            UpdateText(_displayedTime);
        }

        private void OnEnable()
        {
            _healthModel.OnHealthChanged += SetTargetTime;
            _healthModel.OnHit += PlayHitFlash;
        }

        private void OnDisable()
        {
            _healthModel.OnHealthChanged -= SetTargetTime;
            _healthModel.OnHit -= PlayHitFlash;
        }

        private void SetTargetTime(float currentTime) 
            => _targetTime = currentTime;

        private void Update()
        {
            _displayedTime = Mathf.MoveTowards(_displayedTime, _targetTime, smoothSpeed * Time.deltaTime);

            if (_hitTimer > 0f)
                _hitTimer -= Time.deltaTime;

            UpdateText(_displayedTime);
            UpdateVisuals(_displayedTime);
        }
        

        private void UpdateText(float timeToDisplay)
        {
            var mins = Mathf.FloorToInt(timeToDisplay / 60);
            var secs = Mathf.FloorToInt(timeToDisplay % 60);
            textDisplay.text = $"{mins:00}:{secs:00}";
        }

        private void UpdateVisuals(float currentTime)
        {
            if (_hitTimer > 0f)
            {
                var pulse = Mathf.Lerp(0.7f, 1f, Mathf.PingPong(Time.time * 10f, 1f));
                textDisplay.color = new Color(1f, 0f, 0f, pulse);
                return;
            }

            if (currentTime <= 15f)
            {
                var pulse = Mathf.Lerp(0.6f, 1f, Mathf.PingPong(Time.time * 4f, 1f));
                textDisplay.color = new Color(1f, 0.2f, 0.2f, pulse);
                return;
            }

            if (currentTime <= 30f)
            {
                textDisplay.color = new Color(1f, 0.3f, 0.3f, 1f);
                return;
            }

            if (currentTime <= 60f)
            {
                textDisplay.color = new Color(1f, 0.6f, 0.6f, 1f);
                return;
            }

            textDisplay.color = Color.white;
        }
        
        private void PlayHitFlash()
        {
            _hitTimer = hitFlashDuration;
        }
    }
}