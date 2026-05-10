using System;
using System.Threading.Tasks;
using _Project.Runtime.Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

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
        public string GetInteractionLabel() => "Open Door";


        private void Awake()
        {
            Renderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            interactableCollider.isTrigger = false;
        }

        public async void Interact(GameObject initiator, Action onComplete)
        {
            if (_isAnimating) return;
            
            onComplete?.Invoke();
            
            if (isLeverDoor) return;
            
            if (!_isOpen || !IsBlocked())
                await SetDoorStateAsync(!_isOpen);
        }

        public async void InteractFromLever(GameObject initiator, Action onComplete)
        {
            if (_isAnimating) return;
            onComplete?.Invoke();
            if (AreAllLeversActive() && !_isOpen)
                await SetDoorStateAsync(true);
            else if (_isOpen && !IsBlocked())
                await SetDoorStateAsync(false);
        }

        private bool IsBlocked()
        {
            var filter = new ContactFilter2D().NoFilter();
            var results = new Collider2D[5];

            var count = interactableCollider.Overlap(filter, results);

            for (var i = 0; i < count; i++)
                if (results[i].gameObject != gameObject)
                    return true;

            return false;
        }

        private async Task SetDoorStateAsync(bool open)
        {
            _isAnimating = true;
            _animator.SetBool(IsOpen, open);

            await Task.Yield();
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

            if (open)
            {
                await Task.Delay(TimeSpan.FromSeconds(stateInfo.length * 0.4f));
                _isOpen = true;
                interactableCollider.isTrigger = true;
            }
            else
            {
                await Task.Delay(TimeSpan.FromSeconds(stateInfo.length * 0.6f));

                if (IsBlocked())
                {
                    _isOpen = true;
                    interactableCollider.isTrigger = true;
                    _animator.SetBool(IsOpen, true);
                }
                else
                {
                    _isOpen = false;
                    interactableCollider.isTrigger = false;
                }
            }

            _isAnimating = false;
        }

        private bool AreAllLeversActive()
        {
            if (requiredLevers == null || requiredLevers.Length == 0) return false;

            foreach (var lever in requiredLevers)
                if (lever == null || !lever.inActivate)
                    return false;

            return true;
        }


        public void OnHoverEnter()
        {
            var highlightColor = new Color(1.5f, 1.5f, 1.5f, 1f); // HDR White
            if (!isLeverDoor) Renderer.color = highlightColor;
        }

        public void OnHoverExit()
        {
            var normalColor = Color.white;
            if (Renderer) Renderer.color = normalColor;
        }
    }
}