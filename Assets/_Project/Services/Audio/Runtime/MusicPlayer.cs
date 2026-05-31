using UnityEngine;

namespace _Project.Services.Audio.Runtime
{
    public class MusicPlayer : MonoBehaviour
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
            _audioSource.loop = true;
            _audioSource.spatialBlend = 0f;

            return _audioSource;
        }

        public void Play(AudioClip clip, bool loop)
        {
            if (clip == null)
                return;

            if (_audioSource.clip == clip && _audioSource.isPlaying)
                return;

            _audioSource.loop = loop;

            _audioSource.clip = clip;
            _audioSource.Play();
        }

        public void Stop()
        {
            if (_audioSource == null)
                return;

            if (_audioSource.isPlaying)
                _audioSource.Stop();
        }

        public void SetVolume(float volume)
        {
            var source = EnsureAudioSource();
            source.volume = Mathf.Clamp01(volume);
        }
    }
}