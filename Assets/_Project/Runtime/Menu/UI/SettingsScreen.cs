using _Project.Runtime.Menu.Main;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Runtime.Menu.UI
{
    public class SettingsScreen : MonoBehaviour
    {
        [SerializeField] private Button backButton;

        private MenuManager _menuManager;

        [Inject]
        public void Construct(MenuManager menuManager)
        {
            _menuManager = menuManager;
        }

        private void Awake()
        {
            backButton.onClick.AddListener(_menuManager.CloseSettings);
        }

        private void OnDestroy()
        {
            backButton.onClick.RemoveListener(_menuManager.CloseSettings);
        }
    }
}