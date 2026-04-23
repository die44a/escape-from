using _Project.Runtime.Core.Main;
using UnityEngine;
using _Project.Runtime.Player.Main;

namespace _Project.Runtime.Player.Controllers
{
    public class PlayerAnimationController : MonoBehaviour, IGameLateTickable
    {
        private static readonly int WalkKey = Animator.StringToHash("walk");
        private static readonly int DeadKey = Animator.StringToHash("dead");
        private static readonly int DirectionKey = Animator.StringToHash("direction");

        private Animator _animator;
        private PlayerController _playerController;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _playerController = GetComponentInParent<PlayerController>();
        }

        private void OnEnable()
        {
            if (_playerController != null)
                _playerController.OnStateChanged += HandleStateChanged;
        }

        private void OnDisable()
        {
            if (_playerController != null)
                _playerController.OnStateChanged -= HandleStateChanged;
        }

        public void LateTick(float deltaTime)
        {
            if (!_playerController) return;
            
            UpdateDirection(_playerController.MoveInput);
        }

        private void HandleStateChanged(PlayerState newState)
        {
            _animator.SetBool(WalkKey, newState == PlayerState.Walking);
            
            if (newState == PlayerState.Dead)
            {
                _animator.SetTrigger(DeadKey);
            }
        }

        private void UpdateDirection(Vector2 moveInput)
        {
            if (moveInput.sqrMagnitude < 0.01f) return;

            var dir = 0;
            if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
            {
                dir = moveInput.x > 0 ? 3 : 2;
            }
            else
            {
                dir = moveInput.y > 0 ? 1 : 0;
            }

            _animator.SetInteger(DirectionKey, dir);
        }
    }
}