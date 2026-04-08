using System.Collections.Generic;
using _Project.Core.Main;
using UnityEngine;
using Zenject;
// ReSharper disable All

namespace _Project.Core.Runtime.Core.Main
{
    public class GameListenerComposite : MonoBehaviour, 
        IPauseGameListener,
        IResumeGameListener
    {
        [Inject] 
        private GameManager _gameManager;
        
        [InjectLocal]
        private List<IGameListener> _listeners = new();

        void IPauseGameListener.OnPauseGame()
        {
            foreach (var listener in _listeners)
                if (listener is IPauseGameListener startGameListener)
                    startGameListener.OnPauseGame();
        }

        void IResumeGameListener.OnResumeGame()
        {
            foreach (var listener in _listeners)
                if (listener is IResumeGameListener  startGameListener)
                    startGameListener.OnResumeGame();
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