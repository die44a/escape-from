using System.Threading.Tasks;
using _Project.Global;
using _Project.Runtime.Player.Controllers;
using _Project.Runtime.Player.Services;
using _Project.Services;
using UnityEngine;
using Zenject;

namespace _Project.Runtime.Core.Levels
{
    public class LevelFlowService : IInitializable, IExitPointProvider
    {
        private readonly LevelController _levelController;
        private readonly PlayerSpawnService _spawnService;
        private readonly SceneFader _fader;
        private readonly HealthTimeController _healthTimeController;
        private readonly PanelCutscene _cutscene;
        private readonly IInputService _inputService;

        private Transform _currentExit;

        private bool _introPlayed;
        private bool _gameFinished;

        public LevelFlowService(
            LevelController levelController,
            PlayerSpawnService spawnService,
            SceneFader fader,
            HealthTimeController healthTimeController,
            PanelCutscene cutscene,
            IInputService inputService)
        {
            _levelController = levelController;
            _spawnService = spawnService;
            _fader = fader;
            _healthTimeController = healthTimeController;
            _cutscene = cutscene;
            _inputService = inputService;
        }

        void IInitializable.Initialize()
        {
            RunFlow();
        }

        private async void RunFlow()
        {
            await PlayIntro();
        }

        private async Task PlayIntro()
        {
            if (_introPlayed)
                return;

            _introPlayed = true;

            await _fader.FadeOutAsync(1f);

            _inputService.SwitchToUI();

            await _cutscene.PlayAsync();

            _inputService.SwitchToGameplay();

            _levelController.LoadFirstLevel();

            var startPoint = _levelController.GetCurrentSpawnPoint();

            await Task.Yield();

            _spawnService.Spawn(startPoint);

            _currentExit = _levelController.GetCurrentExitPoint();
            
            await _fader.FadeInAsync(1f);
        }

        public async void ChangeLevel()
        {
            if (_gameFinished)
                return;

            await _fader.FadeOutAsync(1f);

            _levelController.LoadNextLevel();

            var nextPoint = _levelController.GetCurrentSpawnPoint();

            await Task.Yield();

            _spawnService.Spawn(nextPoint);

            _currentExit = _levelController.GetCurrentExitPoint();

            _healthTimeController.AddTime(60);

            await Task.Delay(500);

            await _fader.FadeInAsync(1f);

            if (_levelController.GetCurrentExitPoint() == null)
            {
                await FinishGame();
            }
        }

        private async Task FinishGame()
        {
            _gameFinished = true;

            await _fader.FadeOutAsync(1f);

            await Task.Delay(1000);

            Debug.Log("Демка пройдена");

            await Task.Delay(2000);
        }

        Transform IExitPointProvider.GetCurrentExit()
        {
            return _currentExit;
        }
    }
}