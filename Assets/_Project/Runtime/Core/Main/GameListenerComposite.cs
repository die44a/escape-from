using System.Collections.Generic;
using _Project.Runtime.Core.Main;
using UnityEngine;
using Zenject;

// ReSharper disable All
namespace _Project.Core.Runtime.Core.Main
{
    public class ListenerComposite : MonoBehaviour, 
        IGamePauseListener,
        IGameResumeListener,
        IGameStartListener,
        IGameFinishListener
    {
        [Inject] 
        private GameManager _gameManager;
        
        [InjectLocal]
        private List<IGameListener> _listeners = new();
        
        void IGameStartListener.OnStartGame()
        {
            foreach (var listener in _listeners)
                if (listener is IGameStartListener startGameListener)
                    startGameListener.OnStartGame();
        }
        
        void IGameFinishListener.OnFinishGame()
        {
            foreach (var listener in _listeners)
                if (listener is IGameFinishListener gameFinishListener)
                    gameFinishListener.OnFinishGame();
        }
        
        void IGamePauseListener.OnPauseGame()
        {
            foreach (var listener in _listeners)
                if (listener is IGamePauseListener gamePauseListener)
                    gamePauseListener.OnPauseGame();
        }
        
        void IGameResumeListener.OnResumeGame()
        {
            foreach (var listener in _listeners)
                if (listener is IGameResumeListener  gameResumeListener)
                    gameResumeListener.OnResumeGame();
        }
        
        public void Awake()
        {
            _gameManager.AddListener(this);
        }

        public void OnDestroy()
        {
            _gameManager.RemoveListener(this);
        }
    }
}