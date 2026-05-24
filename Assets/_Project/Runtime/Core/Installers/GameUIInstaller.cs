using _Project.Runtime.Core.Levels;
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
        [SerializeField] private UIHint uiHint;
        [SerializeField] private PanelCutscene panelCutscene;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<PauseScreen>()
                .FromInstance(pauseScreen);

            Container.BindInterfacesAndSelfTo<HUDScreen>()
                .FromInstance(hudScreen);
            
            Container.BindInterfacesAndSelfTo<UIHint>()
                .FromInstance(uiHint);
            
            Container.Bind<PanelCutscene>()
                .FromInstance(panelCutscene)
                .AsSingle();
            
            Debug.Log("Game UI installed");
        }
    }
}