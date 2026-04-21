using _Project.Runtime.Core.Camera;
using UnityEngine;
using Zenject;

namespace _Project.Runtime.Core.Installers
{
    public class CameraInstaller : MonoInstaller
    {
        [SerializeField] private CameraPivotController cameraPivot;

        // ReSharper disable Unity.PerformanceAnalysis
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CameraPivotController>()
                .FromInstance(cameraPivot)
                .AsSingle();
        }
    }
}