using _Project.Services;
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
