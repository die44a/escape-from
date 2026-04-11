using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace _Project.Runtime.Menu.Main
{
    public class MenuListenerComposite: MonoBehaviour,
        IGameStartListener
    {
        [Inject] 
        private MenuManager _menuManager;
        
        [InjectLocal]
        private List<IMenuListener> _listeners = new();

        public void OnGameStart()
        {
            foreach (var listener in _listeners)
                if (listener is IGameStartListener  startGameListener)
                    startGameListener.OnGameStart();
        }
        
        public void Awake()
        {
            _menuManager.AddListener(this);
        }

        public void OnDestroy()
        {
            _menuManager.RemoveListener(this);
        }
    }
}