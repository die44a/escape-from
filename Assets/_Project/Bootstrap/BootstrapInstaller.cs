using _Project.Services;
using _Project.Services.Scenes;
using Zenject;

namespace _Project.Bootstrap
{
    public class BootstrapInstaller : MonoInstaller
    {
        [Inject] private SceneLoaderService _sceneLoaderService;
        
        public override void InstallBindings()
        {
        }
        
        public void Initialize()
        {
            _sceneLoaderService.LoadCoreScene();
        }
    }
}
