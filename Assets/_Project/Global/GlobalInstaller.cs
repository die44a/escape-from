using _Project.Services;
using UnityEngine;
using Zenject;

namespace _Project.Global
{
    public class GlobalInstaller : MonoInstaller
    {
        // ReSharper disable Unity.PerformanceAnalysis
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SceneLoaderService>().AsSingle().NonLazy();
        }
    }
}