using System;
using UnityEngine;

namespace _Project.Runtime.Interfaces
{
    public interface IInteractable
    {
        SpriteRenderer Renderer { get; }
        Color HighlightColor => new Color(1.5f, 1.5f, 1.5f, 1f); // HDR White
        Color NormalColor => Color.white;
        
        void Interact(GameObject initiator, Action onComplete = null);        
        bool IsInteractable { get; }
        string GetInteractionLabel();
        
        void OnHoverEnter() 
        {
            if (Renderer) Renderer.color = HighlightColor;
        }
        
        void OnHoverExit() 
        {
            if (Renderer) Renderer.color = NormalColor;
        }
    }
}