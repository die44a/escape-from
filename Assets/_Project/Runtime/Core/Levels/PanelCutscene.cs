using System.Threading.Tasks;
using _Project.Global;
using _Project.Services;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace _Project.Runtime.Core.Levels
{
    public class PanelCutscene : MonoBehaviour
    {
        [SerializeField] private GameObject panelRoot;
        [SerializeField] private GameObject[] panels;

        [Inject] private IInputService _input;
        [Inject] private SceneFader _fader;

        private int _index;
        private bool _isPlaying;

        public async Task PlayAsync()
        {
            if (_isPlaying)
                return;

            _isPlaying = true;

            gameObject.SetActive(true);
            _index = 0;

            HideAll();

            var action = _input.GetAction("UI", "Submit");

            if (action != null)
                action.Enable();

            while (_index < panels.Length)
            {
                await _fader.FadeOutAsync(1f);

                ShowOnly(_index);

                await Task.Yield(); 

                await _fader.FadeInAsync(1f);

                await WaitForInput(action);

                _index++;
            }

            if (action != null)
                action.Disable();

            gameObject.SetActive(false);
            _isPlaying = false;
            panelRoot.SetActive(false);
        }
        
        private void ShowOnly(int index)
        {
            HideAll();
            panels[index].SetActive(true);
        }

        private async Task WaitForInput(InputAction action)
        {
            var tcs = new TaskCompletionSource<bool>();

            void Handler(InputAction.CallbackContext ctx)
            {
                tcs.TrySetResult(true);
            }

            action.performed += Handler;

            await tcs.Task;

            action.performed -= Handler;
        }

        private void HideAll()
        {
            foreach (var p in panels)
                p.SetActive(false);
        }
    }
}