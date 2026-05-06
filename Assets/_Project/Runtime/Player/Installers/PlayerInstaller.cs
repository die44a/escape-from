using _Project.Runtime.Core.Configs;
using _Project.Runtime.Core.Main;
using _Project.Runtime.Player.Controllers;
using _Project.Runtime.Player.Main;
using _Project.Runtime.Player.Services;
using UnityEngine;
using Zenject;

namespace _Project.Runtime.Player.Installers
{
    public class PlayerInstaller : MonoInstaller
    {
        [Header("Player's prefabs")]
        [SerializeField] private GameObject playerPrefab;
        
        [Header("Configurations")]
        [SerializeField] private CoinsConfig coinsConfig;
        
        // ReSharper disable Unity.PerformanceAnalysis
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayerController>()
                .FromComponentInNewPrefab(playerPrefab)
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesAndSelfTo<PlayerAnimationController>()
                .FromComponentInHierarchy(playerPrefab)
                .AsSingle();
            
            Container.BindInterfacesAndSelfTo<HealthTimeController>()
                .FromComponentInHierarchy()
                .AsSingle();
            
            Container.BindInterfacesAndSelfTo<PlayerMovementController>()
                .FromComponentInHierarchy()
                .AsSingle();
            
            Container.BindInterfacesAndSelfTo<PlayerStats>()
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesAndSelfTo<PlayerSpawnService>()
                .AsSingle();
            
            Container.BindInterfacesAndSelfTo<PlayerInteractorController>()
                .FromComponentInHierarchy()
                .AsSingle()
                .NonLazy();
            
            InstallConfigs();
            
            Debug.Log("Player installed");
        }

        private void InstallConfigs()
        {
            Container.BindInstance(coinsConfig)
                .AsSingle();
        }
    }
}