using System;
using UnityEngine;

namespace _Project.Runtime.Interfaces
{
    public interface IInteractable
    {
        SpriteRenderer Renderer { get; }
        Color HighlightColor => new (3f, 3f, 3f, 1f);
        Color NormalColor => Color.white;
        
        void Interact(GameObject initiator, Action onComplete = null);        
        bool IsInteractable { get; }
        string GetInteractionLabel();
        
        void OnHoverEnter() 
        {
            if (!IsInteractable) return;
            if (Renderer) Renderer.color = HighlightColor;
        }
        
        void OnHoverExit() 
        {
            if (Renderer) Renderer.color = NormalColor;
        }
    }
}