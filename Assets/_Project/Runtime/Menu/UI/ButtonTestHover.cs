using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Runtime.Menu.UI
{
    public class ButtonTextHover : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler
    {
        [SerializeField] private TMP_Text buttonText;

        public Color normalColor = Color.white;
        public Color selectedColor = Color.grey;

        private void OnEnable()
        {
            RefreshVisual();
        }

        private void OnDisable()
        {
            SetNormalColor();
        }

        public void OnSelect(BaseEventData eventData) => SetSelectedColor();

        public void OnDeselect(BaseEventData eventData) => SetNormalColor();

        public void OnPointerEnter(PointerEventData eventData)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        public void RefreshVisual()
        {
            if (!buttonText)
                return;

            if (EventSystem.current != null &&
                EventSystem.current.currentSelectedGameObject == gameObject)
                SetSelectedColor();
            else
                SetNormalColor();
        }

        private void SetSelectedColor()
        {
            if (buttonText)
                buttonText.color = selectedColor;
        }

        private void SetNormalColor()
        {
            if (buttonText)
                buttonText.color = normalColor;
        }
    }
}
