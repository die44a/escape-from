using System;
using UnityEngine;
using Zenject;
using _Project.Services.Audio.Configs;
using _Project.Services.Audio.Database;
using _Project.Services.Audio.Runtime;
using AudioSettings = _Project.Services.Audio.Configs.AudioSettings;
namespace _Project.Services.Audio
{
    public class AudioInstaller : MonoInstaller
    {
        [Header("Config")]
        [SerializeField] private AudioSettings _audioSettings;
        [SerializeField] private AudioDatabase _audioDatabase;

        [Header("Scene Players")]
        [SerializeField] private MusicPlayer _musicPlayer;
        [SerializeField] private SfxPlayer _sfxPlayer;
        [SerializeField] private UISfxPlayer _uiSfxPlayer;

        public override void InstallBindings()
        {
            ValidateReferences();
            BindConfigs();
            BindPlayers();
            BindService();
        }

        private void ValidateReferences()
        {
            if (_audioSettings == null)
                throw new InvalidOperationException($"{nameof(AudioInstaller)}: AudioSettings is not assigned.");

            if (_audioDatabase == null)
                throw new InvalidOperationException($"{nameof(AudioInstaller)}: AudioDatabase is not assigned.");

            if (_musicPlayer == null)
                throw new InvalidOperationException($"{nameof(AudioInstaller)}: MusicPlayer is not assigned.");

            if (_sfxPlayer == null)
                throw new InvalidOperationException($"{nameof(AudioInstaller)}: SfxPlayer is not assigned.");

            if (_uiSfxPlayer == null)
                throw new InvalidOperationException($"{nameof(AudioInstaller)}: UISfxPlayer is not assigned.");
        }

        private void BindConfigs()
        {
            Container.Bind<AudioSettings>()
                .FromInstance(_audioSettings)
                .AsSingle();

            Container.Bind<AudioDatabase>()
                .FromInstance(_audioDatabase)
                .AsSingle();
        }

        private void BindPlayers()
        {
            Container.Bind<MusicPlayer>()
                .FromInstance(_musicPlayer)
                .AsSingle();

            Container.Bind<SfxPlayer>()
                .FromInstance(_sfxPlayer)
                .AsSingle();

            Container.Bind<UISfxPlayer>()
                .FromInstance(_uiSfxPlayer)
                .AsSingle();
        }

        private void BindService()
        {
            Container.Bind<IAudioService>()
                .To<AudioService>()
                .AsSingle()
                .NonLazy();
        }
    }
}