using System.Collections;
using _Project.Runtime.Core.Main;
using _Project.Runtime.Menu.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace _Project.Runtime.Core.UI.Pause
{
    public class PauseScreen : MonoBehaviour, 
        IGamePauseListener,
        IGameResumeListener
    {
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button exitToMenuButton; 
        
        private GameManager _gameManager;

        [Inject]
        private void Construct(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        private void Awake()
        {
            resumeButton.onClick.AddListener(OnResumeClicked);
            exitToMenuButton.onClick.AddListener(OnExitClicked);
            
            Hide();
        }

        private void OnDestroy()
        {
            resumeButton.onClick.RemoveListener(OnResumeClicked);
            exitToMenuButton.onClick.RemoveListener(OnExitClicked);
        }
        
        private void Update()
        {
            if (!gameObject.activeSelf)
                return;

            if (!EventSystem.current)
                return;

            if (EventSystem.current.currentSelectedGameObject)
                return;

            if (resumeButton && resumeButton.gameObject.activeInHierarchy)
                EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);
        }

        private void OnResumeClicked()
        {
            _gameManager.ResumeGame();
        }
        
        private IEnumerator SelectDefaultNextFrame()
        {
            yield return null;

            if (!EventSystem.current)
                yield break;

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);

            RefreshTextHovers();
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        private void RefreshTextHovers()
        {
            foreach (var hover in GetComponentsInChildren<ButtonTextHover>(true))
                hover.RefreshVisual();
        }

        private void OnExitClicked()
        {
            _gameManager.ExitToMenu();
        }

        private void Show()
        {
            gameObject.SetActive(true);

            StopAllCoroutines();
            StartCoroutine(SelectDefaultNextFrame());
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        void IGamePauseListener.OnPauseGame() => Show();
        void IGameResumeListener.OnResumeGame() => Hide();
    }
}