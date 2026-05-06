using System;
using System.Collections.Generic;
using _Project.Services;
using _Project.Services.Scenes;
using UnityEngine;
using Zenject;

namespace _Project.Runtime.Menu.Main
{
    public sealed class MenuManager : IInitializable
    {
        [Inject] 
        private SceneLoaderService _sceneLoaderService;

        [Inject]
        private IInputService _inputService;
        
        public event Action OnGameStart;
        
        public MenuState State { get; private set; }
        private readonly List<IMenuListener> _listeners = new();
        
        public void AddListener(IMenuListener listener)
            => _listeners.Add(listener);
        
        public void RemoveListener(IMenuListener listener)
            => _listeners.Remove(listener);

        public void StartGame()
        {
            foreach (var listener in _listeners)
                if (listener is IGameStartListener  startGameListener)
                    startGameListener.OnGameStart();
            
            OnGameStart?.Invoke();
            _sceneLoaderService.LoadCoreScene();
            
            Debug.Log("Game Started");
        }

        void IInitializable.Initialize()
        {
            State = MenuState.MAIN;
            _inputService.SwitchToUI();
            
            Debug.Log("MenuManager initialized");
        }
    }
}
