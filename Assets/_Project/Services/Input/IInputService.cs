using System;
using UnityEngine.InputSystem;

namespace _Project.Services
{
    public interface IInputService
    {
        event Action<string> OnActionMapChanged;

        string CurrentActionMap { get; }
        InputActionAsset Actions { get; }

        void SwitchToGameplay();
        void SwitchToUI();
        void SwitchTo(string mapName);
        InputAction GetAction(string mapName, string actionName);
    }
}
