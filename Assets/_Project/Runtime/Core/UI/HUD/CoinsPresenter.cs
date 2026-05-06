using UnityEngine;
using TMPro;
using Zenject;
using _Project.Runtime.Player.Main;

namespace _Project.Runtime.UI.Hud
{
    public class CoinsPresenter : MonoBehaviour
    {
        private static readonly int CollectTrigger = Animator.StringToHash("collect");
        
        [Header("BronzeCoin")]
        [SerializeField] private TextMeshProUGUI bronzeText;
        [SerializeField] private Animator bronzeAnimator;

        [Header("SilverCoin")]
        [SerializeField] private TextMeshProUGUI silverText;
        [SerializeField] private Animator silverAnimator;

        [Header("GoldCoin")]
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private Animator goldAnimator;

        [Inject] private PlayerStats _stats;

        private int _lastBronze;
        private int _lastSilver;
        private int _lastGold;
        
        private void OnEnable()
        {
            _stats.OnCoinsChanged += UpdateUI;
            
            _lastBronze = _stats.BronzeCoins;
            _lastSilver = _stats.SilverCoins;
            _lastGold = _stats.GoldCoins;
            
            RefreshAllTexts(_lastBronze, _lastSilver, _lastGold);
        }

        private void OnDisable()
        {
            _stats.OnCoinsChanged -= UpdateUI;
        }

        private void UpdateUI(int bronze, int silver, int gold)
        {
            if (bronze != _lastBronze)
            {
                bronzeAnimator.SetTrigger(CollectTrigger);
                bronzeText.text = bronze.ToString();
                _lastBronze = bronze;
            }

            if (silver != _lastSilver)
            {
                silverAnimator.SetTrigger(CollectTrigger);
                silverText.text = silver.ToString();
                _lastSilver = silver;
            }

            if (gold != _lastGold)
            {
                goldAnimator.SetTrigger(CollectTrigger);
                goldText.text = gold.ToString();
                _lastGold = gold;
            }
        }

        private void RefreshAllTexts(int b, int s, int g)
        {
            bronzeText.text = b.ToString();
            silverText.text = s.ToString();
            goldText.text = g.ToString();
        }
    }
}