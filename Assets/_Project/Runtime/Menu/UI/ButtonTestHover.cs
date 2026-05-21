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

        private void Start()
        {
            if (buttonText != null)
                normalColor = buttonText.color;
        }

        public void OnSelect(BaseEventData eventData)
        {
            SetSelectedColor();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            SetNormalColor();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        private void SetSelectedColor()
        {
            if (buttonText != null)
                buttonText.color = selectedColor;
        }

        private void SetNormalColor()
        {
            if (buttonText != null)
                buttonText.color = normalColor;
        }
    }
}