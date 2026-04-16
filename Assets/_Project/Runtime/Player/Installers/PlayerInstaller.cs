using _Project.Runtime.Core.Main;
using _Project.Runtime.Player.Controllers;
using UnityEngine;
using Zenject;

namespace _Project.Runtime.Player.Installers
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private GameObject playerPrefab; 
        
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
            
            Debug.Log("Player installed");

        }
    }
}