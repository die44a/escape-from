using _Project.Runtime.Core.UI;
using _Project.Runtime.Core.UI.HUD;
using _Project.Runtime.Core.UI.Pause;
using UnityEngine;
using Zenject;

// ReSharper disable Unity.PerformanceCriticalCodeInvocation
namespace _Project.Runtime.Core.Installers
{
    public class GameUIInstaller : MonoInstaller
    {
        [SerializeField] private PauseScreen pauseScreen;
        [SerializeField] private HUDScreen hudScreen;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<PauseScreen>().FromInstance(pauseScreen);

            Container.BindInterfacesAndSelfTo<HUDScreen>().FromInstance(hudScreen);
            
            Debug.Log("Game UI installed");
        }
    }
}