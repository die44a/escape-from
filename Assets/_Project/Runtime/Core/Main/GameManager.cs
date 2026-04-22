using System;
using System.Collections.Generic;
using _Project.Runtime.Core.Camera;
using _Project.Runtime.Player.Controllers;
using _Project.Services;
using UnityEngine;
using Zenject;

// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace _Project.Runtime.Core.Main
{
    public sealed class GameManager : IInitializable, IDisposable
    {
        [Inject] private IInputService _inputService;
        [Inject] private CameraPivotController _cameraPivot;
        [Inject] private PlayerController _player;

        public event Action OnPauseGame;
        public event Action OnResumeGame;
        
        public GameState State { get; private set; }
        private readonly List<IGameListener> _listeners = new();
        
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
            
            OnPauseGame?.Invoke();
            
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
            
            OnResumeGame?.Invoke();
            
            Debug.Log($"Game Resumed: {State}");
        }

        private void TogglePause()
        {
            if (State == GameState.PAUSED)
                ResumeGame();
            else
                PauseGame();
        }

        void IInitializable.Initialize()
        {
            _cameraPivot.AttachTo(_player.gameObject);
            
            State = GameState.PLAY;
            _inputService.SwitchToGameplay();
        }   

        void IDisposable.Dispose()
        {
        }
    }
}