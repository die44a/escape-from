using _Project.Runtime.Menu.Main;
using _Project.Services.Settings;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Runtime.Menu.UI
{
    public class SettingsScreen : MonoBehaviour
    {
        [SerializeField] private Button backButton;
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private Toggle fullscreenToggle;

        private MenuManager _menuManager;
        private GameSettingsService _settings;

        [Inject]
        public void Construct(MenuManager menuManager, GameSettingsService settings)
        {
            _menuManager = menuManager;
            _settings = settings;
        }

        private void OnEnable()
        {
            volumeSlider.SetValueWithoutNotify(_settings.MasterVolume);
            fullscreenToggle.SetIsOnWithoutNotify(_settings.IsFullscreen);
        }

        private void Awake()
        {
            backButton.onClick.AddListener(_menuManager.CloseSettings);
            volumeSlider.onValueChanged.AddListener(_settings.SetMasterVolume);
            fullscreenToggle.onValueChanged.AddListener(_settings.SetFullscreen);
        }

        private void OnDestroy()
        {
            backButton.onClick.RemoveListener(_menuManager.CloseSettings);
            volumeSlider.onValueChanged.RemoveListener(_settings.SetMasterVolume);
            fullscreenToggle.onValueChanged.RemoveListener(_settings.SetFullscreen);
        }
    }
}
