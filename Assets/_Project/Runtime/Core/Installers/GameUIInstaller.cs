using _Project.Runtime.Core.UI;
using UnityEngine;
using Zenject;

// ReSharper disable Unity.PerformanceCriticalCodeInvocation
namespace _Project.Runtime.Core.Installers
{
    public class GameUIInstaller : MonoInstaller
    {
        [SerializeField] private PauseScreen pauseScreen;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<PauseScreen>().FromInstance(pauseScreen);
            
            Debug.Log("Game UI installed");
        }
    }
}