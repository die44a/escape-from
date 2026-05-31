using System;
using System.Collections.Generic;
using UnityEngine;
using _Project.Services.Audio.Configs;
using _Project.Services.Audio.Database;
using _Project.Services.Audio.Runtime;
using AudioSettings = _Project.Services.Audio.Configs.AudioSettings;

namespace _Project.Services.Audio
{
    public class AudioService : IAudioService
    {
        private readonly AudioDatabase _database;
        private readonly AudioSettings _settings;
        private readonly MusicPlayer _musicPlayer;
        private readonly SfxPlayer _sfxPlayer;
        private readonly UISfxPlayer _uiSfxPlayer;

        private readonly Dictionary<MusicId, AudioClip> _musicMap = new();
        private readonly Dictionary<SoundId, AudioClip[]> _soundMap = new();

        private float _masterVolume;
        private float _musicVolume;
        private float _sfxVolume;
        private float _uiVolume;

        public AudioService(
            AudioDatabase database,
            AudioSettings settings,
            MusicPlayer musicPlayer,
            SfxPlayer sfxPlayer,
            UISfxPlayer uiSfxPlayer)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _musicPlayer = musicPlayer ?? throw new ArgumentNullException(nameof(musicPlayer));
            _sfxPlayer = sfxPlayer ?? throw new ArgumentNullException(nameof(sfxPlayer));
            _uiSfxPlayer = uiSfxPlayer ?? throw new ArgumentNullException(nameof(uiSfxPlayer));

            _database.Initialize();

            _masterVolume = Mathf.Clamp01(_settings.MasterVolume);
            _musicVolume = Mathf.Clamp01(_settings.MusicVolume);
            _sfxVolume = Mathf.Clamp01(_settings.SfxVolume);
            _uiVolume = Mathf.Clamp01(_settings.UiVolume);

            CacheDatabase();
            ApplyMusicVolume();
        }

        private void CacheDatabase()
        {
            _musicMap.Clear();
            _soundMap.Clear();

            foreach (var entry in _database.Music)
            {
                if (entry == null)
                {
                    Debug.LogError("AudioDatabase contains null music entry.");
                    continue;
                }

                if (entry.Clips == null || entry.Clips.Length == 0)
                {
                    Debug.LogError($"Music entry '{entry.Id}' has no clips.");
                    continue;
                }

                var clip = PickRandomClip(entry.Clips);

                if (clip == null)
                {
                    Debug.LogError($"Music entry '{entry.Id}' resolved null clip.");
                    continue;
                }

                if (!_musicMap.TryAdd(entry.Id, clip))
                {
                    Debug.LogError($"Duplicate MusicId in database: {entry.Id}");
                }
            }

            foreach (var entry in _database.Sounds)
            {
                if (entry == null)
                {
                    Debug.LogError("AudioDatabase contains null sound entry.");
                    continue;
                }

                if (entry.Clips == null || entry.Clips.Length == 0)
                {
                    Debug.LogError($"Sound entry '{entry.Id}' has no clips.");
                    continue;
                }

                if (!_soundMap.TryAdd(entry.Id, entry.Clips))
                {
                    Debug.LogError($"Duplicate SoundId in database: {entry.Id}");
                }
            }
        }

        private float GetEffectiveMusicVolume(float volumeScale = 1f)
        {
            return Mathf.Clamp01(_masterVolume * _musicVolume * Mathf.Clamp01(volumeScale));
        }

        private float GetEffectiveSfxVolume(float volumeScale = 1f)
        {
            return Mathf.Clamp01(_masterVolume * _sfxVolume * Mathf.Clamp01(volumeScale));
        }

        private float GetEffectiveUiVolume(float volumeScale = 1f)
        {
            return Mathf.Clamp01(_masterVolume * _uiVolume * Mathf.Clamp01(volumeScale));
        }

        private AudioClip PickRandomClip(AudioClip[] clips)
        {
            if (clips == null || clips.Length == 0)
                return null;

            if (clips.Length == 1)
                return clips[0];

            return clips[UnityEngine.Random.Range(0, clips.Length)];
        }

        private void ApplyMusicVolume()
        {
            _musicPlayer.SetVolume(GetEffectiveMusicVolume());
        }

        private bool TryGetSoundClip(SoundId soundId, out AudioClip clip)
        {
            clip = null;

            if (!_soundMap.TryGetValue(soundId, out var clips))
            {
                Debug.LogWarning($"SoundId not found: {soundId}");
                return false;
            }

            clip = PickRandomClip(clips);

            if (clip == null)
            {
                Debug.LogWarning($"SoundId '{soundId}' resolved to a null clip.");
                return false;
            }

            return true;
        }

        private bool TryGetMusicClip(MusicId musicId, out AudioClip clip)
        {
            clip = null;

            if (!_musicMap.TryGetValue(musicId, out clip) || clip == null)
            {
                Debug.LogWarning($"MusicId not found: {musicId}");
                return false;
            }

            return true;
        }

        public void PlaySound(SoundId soundId, float volumeScale = 1f)
        {
            if (!TryGetSoundClip(soundId, out var clip))
                return;

            _sfxPlayer.Play(clip, GetEffectiveSfxVolume(volumeScale));
        }

        public void PlayUISound(SoundId soundId, float volumeScale = 1f)
        {
            if (!TryGetSoundClip(soundId, out var clip))
                return;

            _uiSfxPlayer.Play(clip, GetEffectiveUiVolume(volumeScale));
        }

        public void PlayMusic(MusicId musicId, bool loop = true)
        {
            if (!TryGetMusicClip(musicId, out var clip))
                return;

            _musicPlayer.Play(clip, loop);
            ApplyMusicVolume();
        }

        public void StopMusic()
        {
            _musicPlayer.Stop();
        }

        public void SetMasterVolume(float volume)
        {
            _masterVolume = Mathf.Clamp01(volume);
            ApplyMusicVolume();
        }

        public void SetMusicVolume(float volume)
        {
            _musicVolume = Mathf.Clamp01(volume);
            ApplyMusicVolume();
        }

        public void SetSfxVolume(float volume)
        {
            _sfxVolume = Mathf.Clamp01(volume);
        }

        public void SetUiVolume(float volume)
        {
            _uiVolume = Mathf.Clamp01(volume);
        }

        public float GetMasterVolume()
        {
            return _masterVolume;
        }

        public float GetMusicVolume()
        {
            return _musicVolume;
        }

        public float GetSfxVolume()
        {
            return _sfxVolume;
        }

        public float GetUiVolume()
        {
            return _uiVolume;
        }

        public void PlayFootstep()
        {
            if (!_soundMap.TryGetValue(SoundId.Footstep, out var clips))
                return;

            var clip = PickRandomClip(clips);
            if (clip == null)
                return;

            var entry = GetSoundEntry(SoundId.Footstep);

            float pitch = 1f;

            if (entry != null)
            {
                pitch = 1f + UnityEngine.Random.Range(-entry.PitchVariance, entry.PitchVariance);
            }

            float volume = GetEffectiveSfxVolume();
            volume *= UnityEngine.Random.Range(0.9f, 1.0f);

            _sfxPlayer.Play(clip, volume, pitch);
        }

        private SoundEntry GetSoundEntry(SoundId id)
        {
            foreach (var entry in _database.Sounds)
            {
                if (entry.Id == id)
                    return entry;
            }

            return null;
        }
    }
}