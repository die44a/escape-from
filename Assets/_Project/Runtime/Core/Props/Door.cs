using System;
using System.Threading.Tasks;
using _Project.Runtime.Interfaces;
using UnityEngine;

namespace _Project.Runtime.Core.Props
{
    public class Door : MonoBehaviour, IInteractable
    {
        public SpriteRenderer Renderer { get; private set; }

        private Animator _animator;

        [SerializeField] private Collider2D interactableCollider;

        private static readonly int IsOpen = Animator.StringToHash("isOpen");

        private bool _isOpen;
        private bool _isAnimating;

        [SerializeField] private Lever[] requiredLevers;
        [SerializeField] private bool isLeverDoor;

        public bool IsInteractable => !isLeverDoor;

        public string GetInteractionLabel()
            => "Взаимодействовать с дверью";

        private void Awake()
        {
            Renderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            interactableCollider.isTrigger = false;
        }

        public void Interact(GameObject initiator, Action onComplete)
        {
            if (_isAnimating) return;

            onComplete?.Invoke();

            if (isLeverDoor) return;

            SetDoorStateAsync(!_isOpen);
        }

        public void InteractFromLever(GameObject initiator, Action onComplete)
        {
            if (_isAnimating) return;

            onComplete?.Invoke();

            if (AreAllLeversActive() && !_isOpen)
            {
                SetDoorStateAsync(true);
            }
            else if (_isOpen && !IsBlocked())
            {
                SetDoorStateAsync(false);
            }
        }

        private async void SetDoorStateAsync(bool open)
        {
            if (_isAnimating) return;
            _isAnimating = true;

            if (!open && IsBlocked())
            {
                _isAnimating = false;
                return;
            }

            _animator.SetBool(IsOpen, open);

            await Task.Yield();

            float duration = open ? 0.4f : 0.6f;

            await Task.Delay(TimeSpan.FromSeconds(duration));

            if (open)
            {
                interactableCollider.isTrigger = true;
                _isOpen = true;
            }
            else
            {
                interactableCollider.isTrigger = false;

                if (IsBlocked())
                {
                    _animator.SetBool(IsOpen, true);
                    interactableCollider.isTrigger = true;
                    _isOpen = true;
                }
                else
                {
                    _isOpen = false;
                }
            }

            _isAnimating = false;
        }

        private bool IsBlocked()
        {
            var filter = new ContactFilter2D().NoFilter();
            var results = new Collider2D[5];

            var count = interactableCollider.Overlap(filter, results);

            for (var i = 0; i < count; i++)
            {
                if (results[i].gameObject != gameObject)
                    return true;
            }

            return false;
        }

        private bool AreAllLeversActive()
        {
            if (requiredLevers == null || requiredLevers.Length == 0)
                return false;

            foreach (var lever in requiredLevers)
            {
                if (lever == null || !lever.inActivate)
                    return false;
            }

            return true;
        }

        public void OnHoverEnter()
        {
            if (isLeverDoor) return;
            Renderer.color = new Color(1.5f, 1.5f, 1.5f, 1f);
        }

        public void OnHoverExit()
        {
            if (Renderer)
                Renderer.color = Color.white;
        }
    }
}