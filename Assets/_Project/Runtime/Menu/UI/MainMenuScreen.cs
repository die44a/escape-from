using _Project.Runtime.Menu.Main;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Runtime.Menu.UI
{
    public class MainMenuScreen : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button exitButton;

        private MenuManager _menuManager;

        [Inject]
        public void Construct(MenuManager menuManager)
        {
            _menuManager = menuManager;
        }

        private void Awake()
        {
            startButton.onClick.AddListener(_menuManager.StartGame);
            settingsButton.onClick.AddListener(_menuManager.OpenSettings);
            exitButton.onClick.AddListener(_menuManager.ExitGame);
        }

        private void OnDestroy()
        {
            startButton.onClick.RemoveListener(_menuManager.StartGame);
            settingsButton.onClick.RemoveListener(_menuManager.OpenSettings);
            exitButton.onClick.RemoveListener(_menuManager.ExitGame);
        }
    }
}