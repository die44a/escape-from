using TMPro;
using UnityEngine;
using Zenject;

namespace _Project.Runtime.Core.UI.HUD
{
    public class TimeView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textDisplay;
        
        [Inject] private IHealthObservable _healthModel;

        private void OnEnable()
            => _healthModel.OnHealthChanged += RefreshDisplay;
    

        private void OnDisable()
            => _healthModel.OnHealthChanged -= RefreshDisplay;

        private void RefreshDisplay(float currentTime)
        {
            var mins = Mathf.FloorToInt(currentTime / 60);
            var secs = Mathf.FloorToInt(currentTime % 60);
            
            var timeString = $"{mins:00}:{secs:00}";
        
            textDisplay.text = $"{timeString}";

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