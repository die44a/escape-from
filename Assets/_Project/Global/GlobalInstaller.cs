using _Project.Services;
using _Project.Services.Scenes;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace _Project.Global
{
    public class GlobalInstaller : MonoInstaller
    {
        [SerializeField] private InputActionAsset inputActions;
        [SerializeField] private SceneFader fader;

        // ReSharper disable Unity.PerformanceAnalysis
        public override void InstallBindings()
        {
            if (inputActions == null)
                Debug.LogError("GlobalInstaller: InputActionAsset is not assigned.");
            
            if (fader == null)
                Debug.LogError("GlobalInstaller: SceneFader is not assigned.");
            
            Container.Bind<SceneFader>()
                .FromComponentInNewPrefab(fader)
                .AsSingle()
                .NonLazy();

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
            if (fader == null)
                Debug.LogWarning("GlobalInstaller: assign SceneFader in ProjectContext.");
        }
#endif
    }
}