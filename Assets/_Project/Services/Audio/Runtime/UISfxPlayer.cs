using UnityEngine;

namespace _Project.Services.Audio.Runtime
{
    public class UISfxPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;

        private void Awake()
        {
            EnsureAudioSource();
        }

        private void Reset()
        {
            EnsureAudioSource();
        }

        private AudioSource EnsureAudioSource()
        {
            if (_audioSource == null)
                _audioSource = GetComponent<AudioSource>();

            if (_audioSource == null)
                _audioSource = gameObject.AddComponent<AudioSource>();

            _audioSource.playOnAwake = false;
            _audioSource.loop = false;
            _audioSource.spatialBlend = 0f;

            return _audioSource;
        }

        public void Play(AudioClip clip, float volume = 1f)
        {
            if (clip == null)
                return;

            var source = EnsureAudioSource();
            source.PlayOneShot(clip, Mathf.Clamp01(volume));
        }
    }
}