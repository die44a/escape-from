using System.Threading.Tasks;
using _Project.Services;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace _Project.Runtime.Core.Levels
{
    public class PanelCutscene : MonoBehaviour
    {
        [SerializeField] private GameObject[] panels;

        [Inject] private IInputService _input;

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
                panels[_index].SetActive(true);

                await WaitForInput(action);

                panels[_index].SetActive(false);
                _index++;
            }

            if (action != null)
                action.Disable();

            gameObject.SetActive(false);
            _isPlaying = false;
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