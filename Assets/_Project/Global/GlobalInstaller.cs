using _Project.Services;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using InputService = _Project.Services.InputService;

namespace _Project.Global
{
    public class GlobalInstaller : MonoInstaller
    {
        [SerializeField] private InputActionAsset inputActions;

        // ReSharper disable Unity.PerformanceAnalysis
        public override void InstallBindings()
        {
            if (inputActions == null)
                Debug.LogError("GlobalInstaller: InputActionAsset is not assigned.");

            Container.BindInterfacesAndSelfTo<SceneLoaderService>()
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesAndSelfTo<InputService>()
                .AsSingle()
                .WithArguments(inputActions)
                .NonLazy();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (inputActions == null)
                Debug.LogWarning("GlobalInstaller: assign InputActionAsset in ProjectContext.");
        }
#endif
    }
}