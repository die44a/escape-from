using System.Threading.Tasks;
using _Project.Global;
using _Project.Runtime.Player.Controllers;
using _Project.Runtime.Player.Services;
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

        private Transform _currentExit;

        public LevelFlowService(
            LevelController levelController,
            PlayerSpawnService spawnService,
            SceneFader fader,
            HealthTimeController healthTimeController)
        {
            _levelController = levelController;
            _spawnService = spawnService;
            _fader = fader;
            _healthTimeController = healthTimeController;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        void IInitializable.Initialize()
        {
            _levelController.LoadFirstLevel();

            var startPoint = _levelController.GetCurrentSpawnPoint();
            _spawnService.Spawn(startPoint);

            _currentExit = _levelController.GetCurrentExitPoint();
        }

        public async void ChangeLevel()
        {
            await _fader.FadeOutAsync(1f);

            _levelController.LoadNextLevel();

            var nextPoint = _levelController.GetCurrentSpawnPoint();
            _spawnService.Spawn(nextPoint);

            _currentExit = _levelController.GetCurrentExitPoint();

            _healthTimeController.AddTime(60);

            await Task.Delay(1000);

            await _fader.FadeInAsync(1f);
        }

        Transform IExitPointProvider.GetCurrentExit()
        {
            return _currentExit;
        }
    }
}