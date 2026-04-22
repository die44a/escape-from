using _Project.Runtime.Core.Main;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Runtime.Core.UI
{
    public class PauseScreen : MonoBehaviour, 
        IInitializable, 
        IGamePauseListener,
        IGameResumeListener
    {
        [SerializeField] 
        private Button resumeButton;
        
        private GameManager _gameManager;

        [Inject]
        private void Construct(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        private void Show()
        {
            gameObject.SetActive(true);
            resumeButton.onClick.AddListener(_gameManager.ResumeGame);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
            resumeButton.onClick.RemoveListener(_gameManager.ResumeGame);
        }

        void IGamePauseListener.OnPauseGame()
        {
            Show();
        }
        
        void IGameResumeListener.OnResumeGame()
        {
            Hide();
        }
        
        void IInitializable.Initialize()
        {
            Hide();
        }
    }
}