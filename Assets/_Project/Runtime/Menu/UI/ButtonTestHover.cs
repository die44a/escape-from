using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Runtime.Menu.UI
{
    public class ButtonTextHover : MonoBehaviour, ISelectHandler, IDeselectHandler
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
            if (buttonText != null)
                buttonText.color = selectedColor;
        }

        public void OnDeselect(BaseEventData eventData)
        {
            if (buttonText != null)
                buttonText.color = normalColor;
        }
    }
}