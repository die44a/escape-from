using _Project.Runtime.Core.Main;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace _Project.Runtime.Core.UI
{
    public class PauseScreen : MonoBehaviour, 
        IInitializable, 
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

        void IInitializable.Initialize()
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

        private void OnResumeClicked()
        {
            _gameManager.ResumeGame();
        }

        private void OnExitClicked()
        {
            _gameManager.ExitToMenu();
        }

        private void Show()
        {
            gameObject.SetActive(true);
            
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        void IGamePauseListener.OnPauseGame() => Show();
        void IGameResumeListener.OnResumeGame() => Hide();
    }
}