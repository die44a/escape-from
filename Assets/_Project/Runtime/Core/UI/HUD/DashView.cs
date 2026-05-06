using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace _Project.Runtime.Core.UI.HUD
{
    public class DashView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _dashText;
        
        [SerializeField] private Color readyColor = Color.white;
        [SerializeField] private Color cooldownColor = new (0.4f, 0.4f, 0.4f, 1f);

        [Inject] private IDashProvider _dashProvider;
        
        private void Update()
        {
            if (_dashProvider.IsDashReady)
                DisplayReady();
            else
                DisplayCooldown();
        }

        private void DisplayReady()
        {
            _dashText.color = readyColor;
            _dashText.text = "DASH";
            
            _dashText.fontStyle = FontStyles.Bold;
        }

        private void DisplayCooldown()
        {
            _dashText.color = cooldownColor;
            _dashText.fontStyle = FontStyles.Normal;
            
            var remaining = _dashProvider.RemainingDashProgress;
            
            _dashText.text = $"{remaining:F1}s";
        }
    }
}