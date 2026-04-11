using _Project.Services;
using _Project.Services.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace _Project.Player.Runtime
{
    /// <summary>
    /// Движение по плоскости XZ: W/S — вперёд/назад по Z, A/D — влево/вправо по X; диагонали нормализуются.
    /// Ввод через ваш <c>IInputService</c>; карта <c>Gameplay</c>, действие <c>Move</c> (Vector2).
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float gravityMultiplier = 1f;

        private Rigidbody2D _rigidbody2D;
        private IInputService _input;
        private InputAction _moveAction;
        private Vector2 _move;
        private float _verticalVelocity;
        private bool _moveSubscribed;
        
        [Inject]
        public void Construct(IInputService input)
        {
            _input = input;
        }

        private void Start()
        {
            TrySubscribeMove();
            
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void TrySubscribeMove()
        {
            if (_moveSubscribed)
                return;

            if (_input == null)
            {
                Debug.LogError($"[PlayerMovementController] IInputService не внедрён");
                return;
            }

            _moveAction = _input.GetAction(InputMaps.Gameplay, PlayerActions.Move);
            if (_moveAction == null)
            {
                Debug.LogError($"[PlayerMovementController] Не найдено действие {InputMaps.Gameplay}/{PlayerActions.Move}");
                return;
            }

            _moveAction.performed += OnMove;
            _moveAction.canceled += OnMove;
        }

        private void OnMove(InputAction.CallbackContext ctx)
        {
            _move = ctx.ReadValue<Vector2>();
        }

        private void OnDestroy()
        {
            _moveAction.performed -= OnMove;
            _moveAction.canceled -= OnMove;
        }

        private void FixedUpdate()
        {
            if (!_rigidbody2D)
                return;
            
            var direction = _move.normalized;
            
            _rigidbody2D.linearVelocity = direction * moveSpeed;        }
    }
}
