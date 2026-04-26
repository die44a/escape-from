using _Project.Runtime.Core.Main;
using Unity.VisualScripting;
using UnityEngine;

namespace _Project.Runtime.Core.UI.HUD
{
    public class HUDScreen: 
        MonoBehaviour, 
        IGamePauseListener,
        IGameResumeListener
    {
        private void Show()
        {
            gameObject.SetActive(true);
        }
        
        private void Hide()
        {
            gameObject.SetActive(false);
        }
        
        void IGamePauseListener.OnPauseGame() => Hide();
        void IGameResumeListener.OnResumeGame() => Show();
    }
}