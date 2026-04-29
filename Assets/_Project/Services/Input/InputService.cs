using System;
using _Project.Runtime.Core.Main;
using _Project.Services.Input;
using UnityEngine.InputSystem;
using Zenject;

namespace _Project.Services
{
    public sealed class InputService : 
        IInputService, 
        IInitializable, 
        IDisposable
    {
        private readonly InputActionAsset _actionsAsset;

        public event Action<string> OnActionMapChanged;

        public string CurrentActionMap { get; private set; }
        public InputActionAsset Actions { get; private set; }

        public InputService(InputActionAsset actionsAsset)
        {
            _actionsAsset = actionsAsset;
        }

        public void Initialize()
        {
            if (!_actionsAsset)
            {
                UnityEngine.Debug.LogError("InputActionAsset is not assigned in GlobalInstaller");
                return;
            }

            Actions = UnityEngine.Object.Instantiate(_actionsAsset);
            Actions.Disable();

            SwitchToUI();
        }

        public void SwitchToGameplay()
        {
            SwitchTo(InputMaps.Gameplay);
        }

        public void SwitchToUI()
        {
            SwitchTo(InputMaps.UI);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void SwitchTo(string mapName)
        {
            if (!Actions)
                return;

            var map = Actions.FindActionMap(mapName, false);
            if (map == null)
            {
                 UnityEngine.Debug.LogError($"Input map '{mapName}' was not found");
                return;
            }

            if (CurrentActionMap == mapName)
                return;

            Actions.Disable();
            map.Enable();
            CurrentActionMap = mapName;
            OnActionMapChanged?.Invoke(mapName);
        }

        public InputAction GetAction(string mapName, string actionName)
        {
            var map = Actions?.FindActionMap(mapName, false);
            return map?.FindAction(actionName, false);
        }

        public void Dispose()
        {
            if (!Actions)
                return;

            Actions.Disable();
            UnityEngine.Object.Destroy(Actions);
            Actions = null;
        }
    }
}
