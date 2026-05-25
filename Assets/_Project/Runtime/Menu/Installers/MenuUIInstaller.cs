using _Project.Runtime.Menu.UI;
using UnityEngine;
using Zenject;

namespace _Project.Runtime.Menu.Installers
{
    public sealed class MenuUIInstaller : MonoInstaller
    {
        [SerializeField] private MainMenuScreen mainMenuScreen;
        [SerializeField] private MenuScreensController menuScreensController;

        // ReSharper disable Unity.PerformanceAnalysis
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<MainMenuScreen>().FromInstance(mainMenuScreen);
            Container.Bind<MenuScreensController>().FromInstance(menuScreensController);
            
            Debug.Log("Menu UI installed");
        }
    }
}