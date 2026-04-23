using _Project.Runtime.Menu.UI;
using UnityEngine;
using Zenject;

namespace _Project.Runtime.Menu.Installers
{
    public sealed class MenuUIInstaller : MonoInstaller
    {
        [SerializeField] private MainMenuScreen mainMenuScreen;

        // ReSharper disable Unity.PerformanceAnalysis
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<MainMenuScreen>().FromInstance(mainMenuScreen);
            
            Debug.Log("Menu UI installed");
        }
    }
}