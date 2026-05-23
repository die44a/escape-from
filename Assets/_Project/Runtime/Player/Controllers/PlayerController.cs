using System;
using System.Collections;
using _Project.Runtime.Core.Main;
using _Project.Runtime.Player.Main;
using _Project.Services;
using _Project.Services.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace _Project.Runtime.Player.Controllers
{
    public class PlayerController :
        MonoBehaviour,
        IPlayerStatus
    {
        private IInputService _inputService;
        private IHealthObservable _healthObservable;
        private PlayerMovementController _movementController;
        private PlayerInteractorController _interactorController;
        private WeaponSlot _weaponSlot;
        
        public PlayerState CurrentState { get; private set; }
        public Vector2 MoveInput => _moveInput;
        public Vector2 LastDirection => _movementController.LastDirection;
        public bool IsInvulnerableState => CurrentState == PlayerState.Dashing;

        public event Action<PlayerState> OnStateChanged;

        private Vector2 _moveInput;

        private InputAction _moveAction;
        private InputAction _dashAction;
        private InputAction _interactAction;
        private InputAction _attackAction;

        [Inject]
        private void Construct(
            PlayerMovementController movementController,
            IInputService inputService,
            IHealthObservable healthObservable,
            PlayerInteractorController interactorController,
            WeaponSlot weaponSlot)
        {
            _movementController = movementController;
            _inputService = inputService;
            _healthObservable = healthObservable;
            _interactorController = interactorController;
            _weaponSlot = weaponSlot;
        }

        private void Start()
        {
            _moveAction = _inputService.GetAction(InputMaps.Gameplay, PlayerActions.Move);
            _dashAction = _inputService.GetAction(InputMaps.Gameplay, PlayerActions.Dash);
            _interactAction = _inputService.GetAction(InputMaps.Gameplay, PlayerActions.Interact);
            _attackAction = _inputService.GetAction(InputMaps.Gameplay, PlayerActions.Attack);
            
            _dashAction.performed += OnDashPerformed;
            _interactAction.performed += OnInteractPerformed;
            _healthObservable.OnDeath += OnDeath;
            _attackAction.performed += OnAttack;
        }

        private void OnDestroy()
        {
            _dashAction.performed -= OnDashPerformed;
            _interactAction.performed -= OnInteractPerformed;
            _healthObservable.OnDeath -= OnDeath;
            _attackAction.performed -= OnAttack;
        }

        private void OnDashPerformed(InputAction.CallbackContext context)
        {
            if (CurrentState is PlayerState.Dashing
                    or PlayerState.Interacting
                    or PlayerState.Dead)
                return;
            
            if (!_movementController.IsDashReady)
            {
                _movementController.NotifyDashFailed();
                return;
            }

            StartCoroutine(PerformDash());
        }

        private void OnInteractPerformed(InputAction.CallbackContext context)
        {
            if (CurrentState is PlayerState.Dead or PlayerState.Dashing)
                return;

            if (!_interactorController.CanInteract()) return;

            _moveInput = Vector2.zero;
            _movementController.Stop();
            SetState(PlayerState.Interacting);
            _interactorController.PerformInteraction();
        }

        private void Update()
        {
            _moveInput = _moveAction.ReadValue<Vector2>();
        }

        private void FixedUpdate()
        {
            if (CurrentState is PlayerState.Dashing
                or PlayerState.Interacting
                or PlayerState.Dead)
                return;
            
            UpdateMoveState();
            _movementController.ApplyMovement(_moveInput);
        }

        private void UpdateMoveState()
        {
            if (CurrentState == PlayerState.Dead)
                return;

            var targetState = _moveInput.sqrMagnitude > 0.01f
                ? PlayerState.Walking
                : PlayerState.Idle;

            SetState(targetState);
        }

        private IEnumerator PerformDash()
        {
            if (_moveInput.magnitude < 0.01f)
                yield break;

            SetState(PlayerState.Dashing);

            _movementController.Dash(_moveInput);

            yield return new WaitForSeconds(0.2f);

            UpdateMoveState();
        }

        private void SetState(PlayerState newState)
        {
            if (CurrentState == newState) return;

            if (CurrentState == PlayerState.Dead && newState != PlayerState.Dead)
                return;

            CurrentState = newState;
            OnStateChanged?.Invoke(CurrentState);
        }

        private void OnDeath()
        {
            SetState(PlayerState.Dead);
            _movementController.StopPhysics();
            _weaponSlot.DropWeapon();
        }

        private void OnAttack(InputAction.CallbackContext context)
        {
            if (CurrentState is PlayerState.Dead or PlayerState.Dashing)
                return;
            
            _weaponSlot.TryAttack();
        }

        public void ResetPlayer(Vector3 spawnPosition)
        {
            StopAllCoroutines();

            CurrentState = PlayerState.Idle;
            _moveInput = Vector2.zero;

            _movementController.ResetMovement();
            _movementController.Stop();

            transform.position = spawnPosition;

            OnStateChanged?.Invoke(CurrentState);
        }

        public void EndInteraction()
        {
            if (CurrentState == PlayerState.Interacting)
                UpdateMoveState();
        }
    }
}