using System;
using System.Threading.Tasks;
using _Project.Runtime.Interfaces;
using UnityEngine;

namespace _Project.Runtime.Core.Props
{
    public class Door : MonoBehaviour, IInteractable
    {
        [SerializeField] private Collider2D interactableCollider;
        
        private static readonly int IsOpen = Animator.StringToHash("isOpen");
        
        public SpriteRenderer Renderer { get; private set; }
        
        private Animator _animator; 
        private bool _isOpen;
        
        private void Awake()
        {
            Renderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            interactableCollider.isTrigger = false;
        }

        public async void Interact(GameObject initiator, Action onComplete)
        {
            onComplete?.Invoke();
            if (!_isOpen)
                await SetDoorStateAsync(true);
            else
                if (!IsBlocked())
                    await SetDoorStateAsync(false);
        }

        private bool IsBlocked()
        {
            var filter = new ContactFilter2D().NoFilter();
            var results = new Collider2D[5];
    
            var count = interactableCollider.Overlap(filter, results);
    
            for (var i = 0; i < count; i++)
                if (results[i].gameObject != gameObject) return true;

            return false;
        }

        private async Task SetDoorStateAsync(bool open)
        {
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
        }

        public bool IsInteractable => true;
        public string GetInteractionLabel() => "Open Door";
    }
}