using System;
using System.Collections.Generic;
using _Project.Runtime.Core.Camera;
using _Project.Runtime.Player.Controllers;
using _Project.Services;
using _Project.Services.Input;
using _Project.Services.Scenes;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace _Project.Runtime.Core.Main
{
    public sealed class GameManager : IInitializable, IDisposable
    {
        [Inject] private IInputService _inputService;
        [Inject] private CameraPivotController _cameraPivot;
        [Inject] private PlayerController _player;
        [Inject] private SceneLoaderService _sceneLoader;
        
        public GameState State { get; private set; }
        private readonly List<IGameListener> _listeners = new();

        private InputAction _pauseAction;
        
        public void AddListener(IGameListener listener)
            => _listeners.Add(listener);
        
        public void RemoveListener(IGameListener listener)
            => _listeners.Remove(listener);

        public void PauseGame()
        {
            if (State == GameState.PAUSED)
                return;
            
            State = GameState.PAUSED;
            
            foreach (var listener in _listeners)
                if (listener is IGamePauseListener  startGameListener)
                    startGameListener.OnPauseGame();
            
            _inputService.SwitchToUI();
            Time.timeScale = 0f;
            
            Debug.Log($"Game Paused: {State}");
        }

        public void ResumeGame()
        {
            if (State == GameState.PLAY)
                return;
            
            State = GameState.PLAY;
            
            foreach (var listener in _listeners)
                if (listener is IGameResumeListener resumeGameListener)
                    resumeGameListener.OnResumeGame();
            
            _inputService.SwitchToGameplay();
            Time.timeScale = 1f;
            
            Debug.Log($"Game Resumed: {State}");
        }

        public void ExitToMenu()
        {
            Time.timeScale = 1f;
            _inputService.SwitchToUI();
            
            Debug.Log("Exiting to Menu via SceneLoaderService");
            _sceneLoader.LoadMenuScene(); 
        }

        private void TogglePause(InputAction.CallbackContext context)
        {
            if (State == GameState.PAUSED)
                ResumeGame();
            else
                PauseGame();
        }

        void IInitializable.Initialize()
        {
            _cameraPivot.AttachTo(_player.gameObject);
            _pauseAction = _inputService.GetAction(InputMaps.Gameplay, PlayerActions.Pause);
            _pauseAction.performed += TogglePause;

            State = GameState.PLAY;
            _inputService.SwitchToGameplay();
        }   

        void IDisposable.Dispose()
        {
            _pauseAction.performed -= TogglePause;
        }
    }
}