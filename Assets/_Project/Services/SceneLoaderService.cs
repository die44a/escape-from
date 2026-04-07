using UnityEngine;
using Zenject;

namespace _Project.Services
{
    public class SceneLoaderService : IInitializable
    {
        private ZenjectSceneLoader _sceneLoader;

        [Inject]
        private void Construct(ZenjectSceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public void LoadMenuScene()
        {
            _sceneLoader.LoadScene("1.Menu");
        }

        public void LoadCoreScene()
        {
            _sceneLoader.LoadScene("2.Core");
        }

        public void Initialize()
        {
            // change if needed
            LoadCoreScene();
            Debug.Log("Scene Loaded");
        }
    }
}
