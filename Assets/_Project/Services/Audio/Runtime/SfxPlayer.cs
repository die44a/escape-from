using System.Collections.Generic;
using UnityEngine;

namespace _Project.Services.Audio.Runtime
{
    public class SfxPlayer : MonoBehaviour
    {
        [SerializeField, Min(1)] private int _poolSize = 8;

        private readonly List<AudioSource> _sources = new();
        private int _currentIndex;
        private bool _initialized;

        private void Awake()
        {
            EnsureInitialized();
        }

        private void OnValidate()
        {
            _poolSize = Mathf.Max(1, _poolSize);
        }

        private void EnsureInitialized()
        {
            if (_initialized)
                return;

            _initialized = true;
            _sources.Clear();

            var count = Mathf.Max(1, _poolSize);

            for (int i = 0; i < count; i++)
            {
                var source = gameObject.AddComponent<AudioSource>();
                source.playOnAwake = false;
                source.loop = false;
                source.spatialBlend = 0f;
                _sources.Add(source);
            }

            _currentIndex = 0;
        }

        private AudioSource GetNextSource()
        {
            if (_sources.Count == 0)
                return null;

            for (int i = 0; i < _sources.Count; i++)
            {
                var index = (_currentIndex + i) % _sources.Count;
                if (!_sources[index].isPlaying)
                {
                    _currentIndex = (index + 1) % _sources.Count;
                    return _sources[index];
                }
            }

            var fallback = _sources[_currentIndex];
            _currentIndex = (_currentIndex + 1) % _sources.Count;
            return fallback;
        }

        public void Play(AudioClip clip, float volume = 1f)
        {
            if (clip == null)
                return;

            EnsureInitialized();

            var source = GetNextSource();
            if (source == null)
                return;

            source.clip = clip;
            source.loop = false;
            source.volume = Mathf.Clamp01(volume);
            source.Play();
        }

        public void Play(AudioClip clip, float volume = 1f, float pitch = 1f)
        {
            if (clip == null)
                return;

            EnsureInitialized();

            var source = GetNextSource();
            if (source == null)
                return;

            source.clip = clip;
            source.loop = false;
            source.volume = Mathf.Clamp01(volume);
            source.pitch = Mathf.Clamp(pitch, 0.5f, 2f);
            source.Play();
        }
    }
}