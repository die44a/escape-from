using System.Threading.Tasks;
using _Project.Global;
using _Project.Runtime.Core.Main;
using _Project.Runtime.Core.UI.HUD;
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
        private readonly FinalScreen _finalScreen;
        private readonly IInputService _inputService;
        private readonly GameManager _gameManager;

        private Transform _currentExit;

        private bool _introPlayed;
        private bool _gameFinished;

        public LevelFlowService(
            LevelController levelController,
            PlayerSpawnService spawnService,
            SceneFader fader,
            HealthTimeController healthTimeController,
            PanelCutscene cutscene,
            FinalScreen finalScreen,
            IInputService inputService,
            GameManager gameManager)
        {
            _levelController = levelController;
            _spawnService = spawnService;
            _fader = fader;
            _healthTimeController = healthTimeController;
            _cutscene = cutscene;
            _finalScreen = finalScreen;
            _inputService = inputService;
            _gameManager = gameManager;
        }

        void IInitializable.Initialize()
        {
            _healthTimeController.OnDeath += HandleDeath;
            RunFlow();
        }

        private async void RunFlow()
        {
            await PlayIntro();
        }

        private async void HandleDeath()
        {
            if (_gameFinished)
                return;

            _gameFinished = true;

            _gameManager.LockInput();
            _healthTimeController.EnterSafeZone();

            await _fader.FadeOutAsync(1f);

            await Task.Delay(500);

            _gameManager.ExitToMenu();
        }

        private async Task PlayIntro()
        {
            if (_introPlayed)
                return;

            _introPlayed = true;

            await _fader.FadeOutAsync(1f);

            _healthTimeController.EnterSafeZone();

            _inputService.SwitchToUI();

            await _cutscene.PlayAsync();

            _inputService.SwitchToGameplay();

            _levelController.LoadFirstLevel();

            var startPoint = _levelController.GetCurrentSpawnPoint();

            await Task.Yield();

            _spawnService.Spawn(startPoint);

            _currentExit = _levelController.GetCurrentExitPoint();

            _healthTimeController.ExitSafeZone();

            await _fader.FadeInAsync(1f);
        }

        public async void ChangeLevel()
        {
            if (_gameFinished)
                return;

            // Последний уровень пройден
            if (!_levelController.HasNextLevel)
            {
                await FinishGame();
                return;
            }

            await _fader.FadeOutAsync(1f);

            _levelController.LoadNextLevel();

            var nextPoint = _levelController.GetCurrentSpawnPoint();

            await Task.Yield();

            _spawnService.Spawn(nextPoint);

            _currentExit = _levelController.GetCurrentExitPoint();

            _healthTimeController.AddTime(60);

            await Task.Delay(300);

            await _fader.FadeInAsync(1f);
        }

        private async Task FinishGame()
        {
            if (_gameFinished)
                return;

            _gameFinished = true;

            _gameManager.LockInput();

            _healthTimeController.EnterSafeZone();

            await _fader.FadeOutAsync(1f);

            _inputService.SwitchToUI();

            _finalScreen.Show();

            await _fader.FadeInAsync(1f);
        }

        Transform IExitPointProvider.GetCurrentExit()
        {
            return _currentExit;
        }
    }
}