using System.Collections.Generic;
using UnityEngine;
using Zenject;

// ReSharper disable Unity.PerformanceCriticalCodeInvocation
namespace _Project.Runtime.Core.Main
{
    public class GameKernel : MonoKernel
    {
        [Inject]
        private GameManager _gameManager;
        
        [Inject(Optional = true, Source = InjectSources.Local)] 
        private List<IGameTickable> _tiсkables = new();
        
        [Inject(Optional = true, Source = InjectSources.Local)] 
        private List<IGameFixedTickable> _fixedTickables = new();
    
        [Inject(Optional = true, Source = InjectSources.Local)] 
        private List<IGameLateTickable> _lateTickables = new();

        public override void Update()
        {
            base.Update();

            if (_gameManager.State != GameState.PLAY)
                return;
            
            var deltaTime = Time.deltaTime;
            foreach (var tickable in _tiсkables)
            {
                tickable.Tick(deltaTime);
            }
        }

        public override void LateUpdate()
        {
            base.LateUpdate();

            if (_gameManager.State != GameState.PLAY)
                return;
            
            var deltaTime = Time.deltaTime;
            foreach (var tickable in _lateTickables)
            {
                tickable.LateTick(deltaTime);
            }
        }
        
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            if  (_gameManager.State != GameState.PLAY)
                return;
            
            var fixedDeltaTime = Time.fixedDeltaTime;
            foreach (var tickable in _fixedTickables)
            {
                tickable.FixedTick(fixedDeltaTime);
            }
        }
    }
}