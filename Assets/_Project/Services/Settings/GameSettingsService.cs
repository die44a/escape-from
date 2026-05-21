using UnityEngine;
using Zenject;

namespace _Project.Services.Settings
{
    public sealed class GameSettingsService : IInitializable
    {
        private const string MasterVolumeKey = "MasterVolume";
        private const string FullscreenKey = "Fullscreen";

        public float MasterVolume { get; private set; } = 1f;
        public bool IsFullscreen { get; private set; }

        public void Initialize()
        {
            MasterVolume = PlayerPrefs.GetFloat(MasterVolumeKey, 1f);
            AudioListener.volume = MasterVolume;

            IsFullscreen = PlayerPrefs.GetInt(FullscreenKey, Screen.fullScreen ? 1 : 0) == 1;
            ApplyFullscreen(IsFullscreen);
        }

        public void SetMasterVolume(float value)
        {
            MasterVolume = Mathf.Clamp01(value);
            PlayerPrefs.SetFloat(MasterVolumeKey, MasterVolume);
            AudioListener.volume = MasterVolume;
        }

        public void SetFullscreen(bool isFullscreen)
        {
            IsFullscreen = isFullscreen;
            PlayerPrefs.SetInt(FullscreenKey, isFullscreen ? 1 : 0);
            ApplyFullscreen(isFullscreen);
        }

        private static void ApplyFullscreen(bool isFullscreen)
        {
            var mode = isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
            var resolution = Screen.currentResolution;
            Screen.SetResolution(resolution.width, resolution.height, mode);
        }
    }
}
