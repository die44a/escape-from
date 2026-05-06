using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace _Project.Runtime.Core.Levels
{
    public class LevelController
    {
        private readonly DiContainer _container;
        private readonly List<GameObject> _levelPrefabs;
        
        private GameObject _currentLevelInstance;
        private int _currentLevelIndex = 0;

        public LevelController(DiContainer container, List<GameObject> levelPrefabs)
        {
            _container = container;
            _levelPrefabs = levelPrefabs;
        }
        
        public void LoadFirstLevel()
        {
            _currentLevelIndex = 0;
            LoadLevel(_currentLevelIndex);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void LoadNextLevel()
        {
            _currentLevelIndex++;
            
            if (_currentLevelIndex < _levelPrefabs.Count)
                LoadLevel(_currentLevelIndex);
            else
                Debug.Log("Все уровни пройдены");
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void LoadLevel(int index)
        {
            if (_currentLevelInstance)
            {
                Object.Destroy(_currentLevelInstance);
            }

            _currentLevelInstance = _container.InstantiatePrefab(_levelPrefabs[index]);
            
            _currentLevelInstance.transform.position = Vector3.zero;
            
            Debug.Log($"[LevelController] Level {index + 1} loaded.");
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public Vector3 GetCurrentSpawnPoint()
        {
            if (!_currentLevelInstance) return Vector3.zero;

            var spawnPoint = _currentLevelInstance.GetComponentInChildren<SpawnPoint>();
            return spawnPoint ? spawnPoint.transform.position : Vector3.zero;
        }
    }
}