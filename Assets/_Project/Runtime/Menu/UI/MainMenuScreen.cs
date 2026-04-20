using _Project.Runtime.Menu.Main;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Runtime.Menu.UI
{
    public class MainMenuScreen : MonoBehaviour
    {
        [SerializeField] private Button startButton;

        private MenuManager _menuManager;

        [Inject]
        public void Construct(MenuManager menuManager)
        {
            _menuManager = menuManager;
            gameObject.SetActive(true);
        }

        private void Awake()
        {
            startButton.onClick.AddListener(_menuManager.StartGame);
        }

        private void OnDestroy()
        {
            startButton.onClick.RemoveListener(_menuManager.StartGame);
        }
    }
}
