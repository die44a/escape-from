using TMPro;
using UnityEngine;
using Zenject;

namespace _Project.Runtime.Core.UI.HUD
{
    public class TimeView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textDisplay;
        [SerializeField] private float smoothSpeed = 15f;
        
        [Inject] private IHealthObservable _healthModel;

        private float _targetTime;
        private float _displayedTime;

        private void Start()
        {
            _targetTime = _healthModel.CurrentHealth;
            _displayedTime = _targetTime;
            UpdateText(_displayedTime);
        }

        private void OnEnable()
            => _healthModel.OnHealthChanged += SetTargetTime;

        private void OnDisable()
            => _healthModel.OnHealthChanged -= SetTargetTime;

        private void SetTargetTime(float currentTime) 
            => _targetTime = currentTime;

        private void Update()
        {
            _displayedTime = Mathf.MoveTowards(_displayedTime, _targetTime, smoothSpeed * Time.deltaTime);
            
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
            if (currentTime < 20f)
            {
                var alpha = Mathf.PingPong(Time.time * 2, 1);
                textDisplay.color = new Color(1, 0, 0, alpha);
            }
            else
                textDisplay.color = Color.white;
        }
    }
}