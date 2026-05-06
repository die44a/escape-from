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
        
        public PlayerState CurrentState { get; private set; }
        public Vector2 MoveInput => _moveInput;
        public Vector2 LastDirection { get; private set; }
        public bool IsInvulnerableState => CurrentState == PlayerState.Dashing;
        
        public event Action<PlayerState> OnStateChanged;

        private Vector2 _moveInput;
        
        private InputAction _moveAction;
        private InputAction _dashAction;
        private InputAction _interactAction;

        [Inject]
        private void Construct(
            PlayerMovementController movementController,
            IInputService inputService,
            IHealthObservable healthObservable,
            PlayerInteractorController interactorController)
        {
            _movementController = movementController;
            _inputService = inputService;
            _healthObservable = healthObservable;
            _interactorController = interactorController;
        }

        private void Start()
        {
            _moveAction = _inputService.GetAction(InputMaps.Gameplay, PlayerActions.Move);
            _dashAction = _inputService.GetAction(InputMaps.Gameplay, PlayerActions.Dash);
            _interactAction = _inputService.GetAction(InputMaps.Gameplay, PlayerActions.Interact);

            _dashAction.performed += OnDashPerformed;
            _interactAction.performed += OnInteractPerformed;
            _healthObservable.OnDeath += OnDeath;
        }

        private void OnDestroy()
        {
            _dashAction.performed -= OnDashPerformed;
            _interactAction.performed -= OnInteractPerformed;
            _healthObservable.OnDeath -= OnDeath;
        }

        private void OnDashPerformed(InputAction.CallbackContext context)
        {
            if (CurrentState is PlayerState.Dashing 
                or PlayerState.Interacting
                or PlayerState.Dead
                || !_movementController.IsDashReady)
                return;
            
            StartCoroutine(PerformDash());
        }

        private void OnInteractPerformed(InputAction.CallbackContext context)
        {
            if (CurrentState is PlayerState.Dead or PlayerState.Dashing)
                return;

            if (_interactorController.CanInteract()) 
            {
                SetState(PlayerState.Interacting);
                
                _interactorController.PerformInteraction();
            }
        }

        public void FixedUpdate()
        {
            if (CurrentState is PlayerState.Dashing 
                or PlayerState.Interacting 
                or PlayerState.Dead)
                return;

            _moveInput = _moveAction.ReadValue<Vector2>();
            
            if (_moveInput.magnitude > 0.01f)
                LastDirection = _moveInput;
            
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