using System.Collections;
using _Project.Runtime.Menu.Main;
using _Project.Services;
using _Project.Services.Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

namespace _Project.Runtime.Menu.UI
{
    public sealed class MenuScreensController : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenuRoot;
        [SerializeField] private GameObject settingsMenuRoot;
        [SerializeField] private Button mainMenuDefaultButton;
        [SerializeField] private Button settingsDefaultButton;

        private MenuManager _menuManager;
        private IInputService _inputService;
        private InputAction _cancelAction;

        [Inject]
        private void Construct(MenuManager menuManager, IInputService inputService)
        {
            _menuManager = menuManager;
            _inputService = inputService;
        }

        private void OnEnable()
        {
            _menuManager.OnStateChanged += ApplyState;
            ApplyState();
            
            StartCoroutine(ForceInitialSelection());
        }
        
        private void Update()
        {
            if (!EventSystem.current)
                return;

            if (EventSystem.current.currentSelectedGameObject)
                return;

            var defaultButton = _menuManager.State == MenuState.MAIN
                ? mainMenuDefaultButton
                : settingsDefaultButton;

            if (defaultButton && defaultButton.gameObject.activeInHierarchy)
                EventSystem.current.SetSelectedGameObject(defaultButton.gameObject);
        }
        
        private IEnumerator ForceInitialSelection()
        {
            yield return null;
            yield return null;

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(mainMenuDefaultButton.gameObject);
        }

        private void OnDisable()
        {
            _menuManager.OnStateChanged -= ApplyState;
            if (_cancelAction != null)
                _cancelAction.performed -= OnCancelPerformed;
        }

        private void OnCancelPerformed(InputAction.CallbackContext _)
        {
            if (_menuManager.State == MenuState.SETTINGS)
                _menuManager.CloseSettings();
        }

        private void ApplyState()
        {
            var isMain = _menuManager.State == MenuState.MAIN;

            mainMenuRoot.SetActive(isMain);
            settingsMenuRoot.SetActive(!isMain);

            StopAllCoroutines();
            StartCoroutine(SelectDefaultNextFrame(isMain));
        }

        private IEnumerator SelectDefaultNextFrame(bool isMain)
        {
            yield return null;

            if (!EventSystem.current)
                yield break;

            var defaultButton = isMain ? mainMenuDefaultButton : settingsDefaultButton;
            if (!defaultButton)
                yield break;

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(defaultButton.gameObject);

            RefreshTextHovers(mainMenuRoot);
            RefreshTextHovers(settingsMenuRoot);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private static void RefreshTextHovers(GameObject root)
        {
            if (!root)
                return;

            foreach (var hover in root.GetComponentsInChildren<ButtonTextHover>(true))
                hover.RefreshVisual();
        }
    }
}
