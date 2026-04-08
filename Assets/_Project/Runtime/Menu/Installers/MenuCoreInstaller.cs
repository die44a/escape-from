using _Project.Runtime.Menu.Main;
using UnityEngine;
using Zenject;

namespace _Project.Runtime.Menu.Installers
{
    public class MenuCoreInstaller : MonoInstaller
    {
        // ReSharper disable Unity.PerformanceAnalysis
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MenuManager>().AsSingle().NonLazy();
            
            Debug.Log("Menu service installed");
        }
    }
}