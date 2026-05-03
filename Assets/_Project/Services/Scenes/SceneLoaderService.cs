using System.Threading.Tasks;
using _Project.Global;
using UnityEngine;
using Zenject;

namespace _Project.Services.Scenes
{
    public class SceneLoaderService
    {
        private ZenjectSceneLoader _sceneLoader;
        private SceneFader _fader;

        private SceneLoaderService(
            ZenjectSceneLoader sceneLoader, 
            SceneFader fader)
        {
            _sceneLoader = sceneLoader;
            _fader = fader;
        }

        public async void LoadMenuScene()
        {
            await _fader.FadeOutAsync(1);
            _sceneLoader.LoadScene("1.Menu");
            await Task.Delay(1000);
            await _fader.FadeInAsync(1);
        }

        public async void LoadCoreScene()
        {
            await _fader.FadeOutAsync(1);
            _sceneLoader.LoadScene("2.Core");
            await Task.Delay(1000);
            await _fader.FadeInAsync(1);
        }
    }
}
