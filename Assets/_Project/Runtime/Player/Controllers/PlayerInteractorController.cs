using _Project.Runtime.Core.UI.HUD;
using _Project.Runtime.Interfaces;
using UnityEngine;
using Zenject;

namespace _Project.Runtime.Player.Controllers
{
    public class PlayerInteractorController : MonoBehaviour
    {
        [SerializeField] private float interactDistance = 1.5f;
        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private Vector2 originOffset = new (0, 0.5f);

        private PlayerController _playerController;
        private UIHint _uiHint;
        
        private IInteractable _currentHoveredInteractable;

        [Inject]
        public void Construct(PlayerController playerController, UIHint uiHint)
        {
            _playerController = playerController;
            _uiHint = uiHint;
        }

        private void FixedUpdate()
        {
            CheckForHover();
        }

        private void CheckForHover()
        {
            var origin = (Vector2)transform.position + originOffset;
            
            var colliders = Physics2D.OverlapCircleAll(origin, interactDistance, interactableLayer);

            IInteractable closestInteractable = null;
            var minDistance = float.MaxValue;

            foreach (var col in colliders)
            {
                if (col.TryGetComponent(out IInteractable interactable))
                {
                    var distance = Vector2.Distance(origin, col.transform.position);

                    if (interactable.IsInteractable && distance < minDistance)
                    {
                        minDistance = distance;
                        closestInteractable = interactable;
                    }
                }
            }
    
            if (closestInteractable != _currentHoveredInteractable)
            {
                _currentHoveredInteractable?.OnHoverExit();
                _currentHoveredInteractable = closestInteractable;
                _currentHoveredInteractable?.OnHoverEnter();
                UpdateUIHint();
            }
        }
        
        private void UpdateUIHint()
        {
            if (_currentHoveredInteractable != null)
                _uiHint.Show($"[E] {_currentHoveredInteractable.GetInteractionLabel()}");
            else
                _uiHint.Hide();
        }

        public void PerformInteraction() 
        {
            if (!CanInteract()) 
                return;

            _currentHoveredInteractable.Interact(gameObject, () => {
                _playerController.EndInteraction(); 
            });
            
            UpdateUIHint();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere((Vector2)transform.position + originOffset, interactDistance);
        }
        
        public bool CanInteract() 
            => _currentHoveredInteractable is { IsInteractable: true };
    }
}