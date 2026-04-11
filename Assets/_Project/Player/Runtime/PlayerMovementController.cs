using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace TombBounders.Player
{
    /// <summary>
    /// Движение по плоскости XZ: W/S — вперёд/назад по Z, A/D — влево/вправо по X; диагонали нормализуются.
    /// Ввод через ваш <c>IInputService</c>; карта <c>Gameplay</c>, действие <c>Move</c> (Vector2).
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovementController : MonoBehaviour
    {
        private const string GameplayMapName = "Gameplay";
        private const string MoveActionName = "Move";

        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float _moveSpeed = 6f;
        [SerializeField] private float _gravityMultiplier = 1f;

        private IInputService _input;
        private InputAction _moveAction;
        private Vector2 _move;
        private float _verticalVelocity;
        private bool _moveSubscribed;

        private void Reset()
        {
            _characterController = GetComponent<CharacterController>();
        }

        [Inject]
        public void Construct(IInputService input)
        {
            _input = input;
        }

        /// <summary>Подписка на ввод (без дублей). Можно вызывать из IInitializable.Initialize вместо Start.</summary>
        public void Initialize()
        {
            TrySubscribeMove();
        }

        private void Start()
        {
            if (_characterController == null)
                _characterController = GetComponent<CharacterController>();

            TrySubscribeMove();
        }

        private void TrySubscribeMove()
        {
            if (_moveSubscribed)
                return;

            if (_input == null)
            {
                Debug.LogError("[PlayerMovementController] IInputService не внедрён (ZenAutoInjecter / контекст).");
                return;
            }

            _moveAction = _input.GetAction(GameplayMapName, MoveActionName);
            if (_moveAction == null)
            {
                Debug.LogError("[PlayerMovementController] Не найдено действие Gameplay/Move.");
                return;
            }

            _moveAction.performed += OnMove;
            _moveAction.canceled += OnMove;
            _moveSubscribed = true;
        }

        private void OnMove(InputAction.CallbackContext ctx)
        {
            _move = ctx.ReadValue<Vector2>();
        }

        public void Dispose()
        {
            if (!_moveSubscribed || _moveAction == null)
                return;

            _moveAction.performed -= OnMove;
            _moveAction.canceled -= OnMove;
            _moveAction = null;
            _moveSubscribed = false;
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void Update()
        {
            if (_characterController == null)
                return;

            var horizontal = new Vector3(_move.x, 0f, _move.y);
            if (horizontal.sqrMagnitude > 1f)
                horizontal.Normalize();

            if (_characterController.isGrounded && _verticalVelocity < 0f)
                _verticalVelocity = -2f;

            _verticalVelocity += Physics.gravity.y * _gravityMultiplier * Time.deltaTime;

            var delta = horizontal * (_moveSpeed * Time.deltaTime);
            delta.y = _verticalVelocity * Time.deltaTime;
            _characterController.Move(delta);
        }
    }
}
