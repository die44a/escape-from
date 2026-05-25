using System.Collections;
using _Project.Runtime.Core.Main;
using _Project.Runtime.Player.Main;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace _Project.Runtime.Core.UI.HUD
{
    public class FinalScreen : MonoBehaviour
    {
        [SerializeField] private GameObject root;

        [Header("Texts")]
        [SerializeField] private TMP_Text bronzeText;
        [SerializeField] private TMP_Text silverText;
        [SerializeField] private TMP_Text goldText;
        [SerializeField] private TMP_Text totalScoreText;

        [Header("Buttons")]
        [SerializeField] private Button exitButton;

        [Header("Animation")]
        [SerializeField] private float countDuration = 1.5f;

        [Inject] private GameManager _gameManager;
        [Inject] private PlayerStats _stats;

        private void Awake()
        {
            exitButton.onClick.AddListener(OnExitClicked);

            root.SetActive(false);
            exitButton.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            exitButton.onClick.RemoveListener(OnExitClicked);
        }

        public void Show()
        {
            root.SetActive(true);
            exitButton.gameObject.SetActive(false);

            StopAllCoroutines();
            StartCoroutine(SelectDefaultNextFrame());
            StartCoroutine(CountScoreRoutine());
        }

        private IEnumerator CountScoreRoutine()
        {
            var bronze = _stats.BronzeCoins;
            var silver = _stats.SilverCoins;
            var gold = _stats.GoldCoins;

            var targetScore = _stats.CalculateTotalScore();

            bronzeText.text = "0";
            silverText.text = "0";
            goldText.text = "0";
            totalScoreText.text = "0";

            var elapsed = 0f;

            while (elapsed < countDuration)
            {
                elapsed += Time.unscaledDeltaTime;

                var t = Mathf.Clamp01(elapsed / countDuration);

                bronzeText.text =
                    $"{Mathf.RoundToInt(bronze * t)}";

                silverText.text =
                    $"{Mathf.RoundToInt(silver * t)}";

                goldText.text =
                    $"{Mathf.RoundToInt(gold * t)}";

                totalScoreText.text =
                    $"{Mathf.RoundToInt(targetScore * t)}";

                yield return null;
            }

            bronzeText.text = $"{bronze}";
            silverText.text = $"{silver}";
            goldText.text = $"{gold}";
            totalScoreText.text = $"{targetScore}";

            exitButton.gameObject.SetActive(true);
        }
        
        private IEnumerator SelectDefaultNextFrame()
        {
            yield return null;

            if (!EventSystem.current)
                yield break;

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(exitButton.gameObject);
        }

        private void OnExitClicked()
        {
            _gameManager.ExitToMenu();
        }
    }
}