using System;
using _Project.Runtime.Interfaces;
using UnityEngine;

namespace _Project.Runtime.Core.Props
{
    public class SafeRune : MonoBehaviour, IInteractable
    {
        [SerializeField] private CircleCollider2D safeZone;
        [SerializeField] private Animator animator;

        private bool _isActivated;

        private static readonly int Activate =
            Animator.StringToHash("activate");

        public SpriteRenderer Renderer { get; private set; }

        public bool IsInteractable => !_isActivated;

        public string GetInteractionLabel()
            => "Активировать рунический камень";

        private void Awake()
        {
            Renderer = GetComponent<SpriteRenderer>();

            if (safeZone)
                safeZone.enabled = false;
        }

        public void Interact(GameObject initiator, Action onComplete = null)
        {
            if (_isActivated)
                return;

            _isActivated = true;

            if (animator)
                animator.SetTrigger(Activate);

            if (safeZone)
                safeZone.enabled = true;

            onComplete?.Invoke();
        }
    }
}