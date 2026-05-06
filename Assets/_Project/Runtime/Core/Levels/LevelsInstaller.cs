using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace _Project.Runtime.Core.Levels
{
    public class LevelsInstaller : MonoInstaller
    {
        [SerializeField] private List<GameObject> levelPrefabs;
        
        // ReSharper disable Unity.PerformanceAnalysis
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<LevelController>()
                .AsSingle();
            
            Container.BindInstance(levelPrefabs)
                .WhenInjectedInto<LevelController>();
            
            Container.BindInterfacesAndSelfTo<LevelFlowService>()
                .AsSingle()
                .NonLazy();
        }
    }
}