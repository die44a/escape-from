using UnityEngine;

namespace _Project.Services.Audio.Configs
{
    [CreateAssetMenu(
        fileName = "AudioSettings",
        menuName = "Project/Audio/Audio Settings")]
    public class AudioSettings : ScriptableObject
    {
        [SerializeField, Range(0f, 1f)]
        private float _masterVolume = 1f;

        [SerializeField, Range(0f, 1f)]
        private float _musicVolume = 1f;

        [SerializeField, Range(0f, 1f)]
        private float _sfxVolume = 1f;

        [SerializeField, Range(0f, 1f)]
        private float _uiVolume = 1f;

        public float MasterVolume => _masterVolume;
        public float MusicVolume => _musicVolume;
        public float SfxVolume => _sfxVolume;
        public float UiVolume => _uiVolume;

        private void OnValidate()
        {
            _masterVolume = Mathf.Clamp01(_masterVolume);
            _musicVolume = Mathf.Clamp01(_musicVolume);
            _sfxVolume = Mathf.Clamp01(_sfxVolume);
            _uiVolume = Mathf.Clamp01(_uiVolume);
        }
    }
}