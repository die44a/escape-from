using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Services.Audio.Database
{
    [CreateAssetMenu(
        fileName = "AudioDatabase",
        menuName = "Project/Audio/Audio Database")]
    public class AudioDatabase : ScriptableObject
    {
        [SerializeField] private List<SoundEntry> _sounds = new();
        [SerializeField] private List<MusicEntry> _music = new();

        private Dictionary<SoundId, SoundEntry> _soundMap;
        private Dictionary<MusicId, MusicEntry> _musicMap;

        private bool _initialized;

        public void Initialize()
        {
            if (_initialized)
                return;

            _initialized = true;

            _soundMap = new Dictionary<SoundId, SoundEntry>(_sounds.Count);
            _musicMap = new Dictionary<MusicId, MusicEntry>(_music.Count);

            BuildSoundMap();
            BuildMusicMap();
        }

        private void BuildSoundMap()
        {
            foreach (var sound in _sounds)
            {
                if (!_soundMap.TryAdd(sound.Id, sound))
                {
                    Debug.LogError($"Duplicate SoundId: {sound.Id}");
                }
            }
        }

        private void BuildMusicMap()
        {
            foreach (var music in _music)
            {
                if (!_musicMap.TryAdd(music.Id, music))
                {
                    Debug.LogError($"Duplicate MusicId: {music.Id}");
                }
            }
        }

        public bool TryGetSound(SoundId id, out SoundEntry entry)
        {
            EnsureInitialized();
            return _soundMap.TryGetValue(id, out entry);
        }

        public bool TryGetMusic(MusicId id, out MusicEntry entry)
        {
            EnsureInitialized();
            return _musicMap.TryGetValue(id, out entry);
        }

        private void EnsureInitialized()
        {
            if (!_initialized)
            {
                Debug.LogError("AudioDatabase is not initialized. Call Initialize() in composition root.");
            }
        }

        public IReadOnlyList<SoundEntry> Sounds => _sounds;
        public IReadOnlyList<MusicEntry> Music => _music;
    }
}