using _Project.Runtime.Core.Main.Interfaces;
using _Project.Services;
using _Project.Services.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using System.Linq;

namespace _Project.Player.Runtime
{
    public class PlayerInteractor : MonoBehaviour
    {
        // private float distanceInteract;

        [Header("Настройки")]
        [SerializeField] private float _interactionRange = 100f;
        [SerializeField] private LayerMask _interactableLayer = 1 << 8; // Layer 8: Interactable
        
        [Header("Ссылки (Опционально)")]
        [SerializeField] private Camera _camera;

        private IInputService _inputService;
        private InputAction _interactAction;
        private IInteractable _currentInteractable;

        public IInteractable CurrentInteractable => _currentInteractable;

        [Inject]
        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        private void Start()
        {
            if (_camera == null)
                _camera = Camera.main;

            _interactAction = _inputService.GetAction(InputMaps.Gameplay, PlayerActions.Interact);
            
            if (_interactAction != null)
            {
                _interactAction.started += _ => Debug.Log("1. [PlayerInteractor] Кнопка взаимодействия НАЖАТА"); // Этот основной
                _interactAction.performed += OnInteractPerformed;
                _interactAction.canceled += _ => Debug.Log("2. [PlayerInteractor] Кнопка взаимодействия ОТПУЩЕНА"); // Этот основной
            }
            else
            {
                Debug.LogError($"3. [PlayerInteractor] Не найдено действие {PlayerActions.Interact}");
            }
        }

        private void OnDestroy()
        {
            if (_interactAction != null)
            {
                _interactAction.performed -= OnInteractPerformed;
            }
        }

        private void Update()
        {
            CheckForInteractable();
            
            // Визуализация луча (только для отладки)
            if (_camera != null)
            {
                Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                Debug.DrawRay(ray.origin, ray.direction * _interactionRange, Color.green);
            }
        }

        private void CheckForInteractable()
        {
            IInteractable foundInteractable = null;

            if (_camera != null)
            {
                Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, _interactionRange, _interactableLayer);

                if (hit.collider != null)
                {
                    foundInteractable = hit.collider.GetComponent<IInteractable>();
                    if (foundInteractable != null)
                    {
                        Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);
                    }
                }
            }
            
            // Режим "Близкого поиска" (OverlapCircle)
            // if (foundInteractable == null)
            // {
            //     Collider2D hit = Physics2D.OverlapCircle(transform.position, interactable.GetInteractionDistance(), _interactableLayer);
            //     Debug.Log($"OEIJFOEFJOEJFOEIJF");

            //     if (hit != null)
            //     {
            //         foundInteractable = hit.GetComponent<IInteractable>();
            //         Debug.Log($"OEIJFOEFJOEJFOEIJF");

            //     }
            // }
            if (foundInteractable == null)
            {   
                                // var allInteractables = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
                                // IInteractable closest = null;
                                // float minDistance = float.MaxValue;

                                // foreach (var mono in allInteractables)
                                // {
                                //     if (mono is IInteractable interactable)
                                //     {
                                //         var col = mono.GetComponentInChildren<Collider2D>();
                                //         float dist = Vector2.Distance(transform.position, col.bounds.center);
                                //         if (dist < minDistance && dist < interactable.GetInteractionDistance()) // Радиус 3 метров
                                //         {
                                //             minDistance = dist;
                                //             closest = interactable;
                                //             Debug.Log($"цщуращшцурапщцурацщшращшцрзыфовщцшораущцшрДоходит цикл сюда?");
                                //         }
                                //     }
                                // }
                IInteractable closest = FindClosestInteractable();
                if (closest != null)
                {
                    Debug.Log($"Вижу дверь видимо");
                    foundInteractable = closest;
                    _currentInteractable?.OnHoverExit(gameObject);
                    _currentInteractable = foundInteractable;
                    _currentInteractable?.OnHoverEnter(gameObject);
                    // closest.Interact(gameObject); // закрывает дверь
                }
            }

            // if (foundInteractable != _currentInteractable)
            // {
            //     _currentInteractable?.OnHoverExit(gameObject);
            //     _currentInteractable = foundInteractable;
            //     _currentInteractable?.OnHoverEnter(gameObject);
                
            //     if (_currentInteractable != null)
            //         Debug.Log($"4. [PlayerInteractor] Вижу объект: {((MonoBehaviour)_currentInteractable).name}"); // Этот основной
            //         // просто вижу дверь
                
            // }
            if (foundInteractable == null && _currentInteractable != null)
            {
                _currentInteractable.OnHoverExit(gameObject);
                _currentInteractable = null;
            }
        }

        private void OnInteractPerformed(InputAction.CallbackContext context)
        {
            Debug.Log("5. [PlayerInteractor] Сигнал Interact получен через Input System"); // Этот основной
            PerformInteraction();
        }

        private void PerformInteraction()
        {
            // 1. Взаимодействуем с тем, на что смотрим
            if (_currentInteractable != null)
            {
                Debug.Log($"6.  [PlayerInteractor] Взаимодействую с целью: {((MonoBehaviour)_currentInteractable).name}"); // Этот основной
                _currentInteractable.Interact(gameObject);  // открывает дверь
                return;
            }

            // 2. Если никого не видим, ищем ТОЛЬКО БЛИЖАЙШЕГО в радиусе 3 метров
                            // var allInteractables = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
                            // IInteractable closest = null;
                            // float minDistance = float.MaxValue;

                            // foreach (var mono in allInteractables)
                            // {
                            //     if (mono is IInteractable interactable)
                            //     {
                            //         var col = mono.GetComponentInChildren<Collider2D>();
                            //         float dist = Vector2.Distance(transform.position,col.bounds.center);
                            //         if (dist < minDistance && dist < interactable.GetInteractionDistance()) // Радиус 3 метров
                            //         {
                            //             minDistance = dist;
                            //             closest = interactable;
                            //             Debug.Log($"222Доходит цикл сюда?");
                            //         }
                            //     }
                            // }
            IInteractable closest = FindClosestInteractable();
            if (closest != null)
            {
                // Debug.Log($"7. [PlayerInteractor] Взаимодействую с ближайшим объектом: {((MonoBehaviour)closest).name} (дистанция: {minDistance:F1})"); // Этот основной
                closest.Interact(gameObject); // закрывает дверь
            }
            else
            {
                Debug.LogWarning("8. [PlayerInteractor] Нажато E, но рядом нет ничего интерактивного."); // Этот основной
                // не с чем взаимподействовать
            }
        }
                        // private IInteractable FindClosestInteractable()
                        // {
                        //     var allInteractables = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
                        //     IInteractable closest = null;
                        //     float minDistance = float.MaxValue;

                        //     foreach (var mono in allInteractables)
                        //     {
                        //         if (mono is IInteractable interactable)
                        //         {
                        //             var col = mono.GetComponentInChildren<Collider2D>();
                        //             float dist = Vector2.Distance(transform.position,col.bounds.center);
                        //             if (dist < minDistance && dist < interactable.GetInteractionDistance()) // Радиус 3 метров
                        //             {
                        //                 minDistance = dist;
                        //                 closest = interactable;
                        //                 Debug.Log($"222Доходит цикл сюда?");
                        //             }       
                        //         }
                        //     } 
                        //     return closest;
                        // }
        [SerializeField] private float _maxSearchRadius = 10f;
        // [SerializeField] private LayerMask _interactableLayer;

        private readonly Collider2D[] _results = new Collider2D[16];

        private IInteractable FindClosestInteractable()
        {
            int count = Physics2D.OverlapCircleNonAlloc(
                transform.position,
                _maxSearchRadius,
                _results,
                _interactableLayer);

            IInteractable closest = null;
            float minDistance = float.MaxValue;

            for (int i = 0; i < count; i++)
            {
                var col = _results[i];

                if (!col.TryGetComponent(out InteractableProxy proxy))
                    continue;

                var interactable = proxy.target as IInteractable;
                if (interactable == null)
                    continue;

                float dist = Vector2.Distance(transform.position, col.bounds.center);

                if (dist < minDistance && dist < interactable.GetInteractionDistance())
                {
                    minDistance = dist;
                    closest = interactable;
                }
            }

            return closest;
        }
     }
    
}
