using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Runtime.Menu.UI
{
    public class ButtonTextHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TMP_Text buttonText;

        public Color normalColor = Color.white;
        public Color hoverColor = Color.grey;

        void Start()
        {
            if (buttonText != null)
                normalColor = buttonText.color;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (buttonText != null)
                buttonText.color = hoverColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (buttonText != null)
                buttonText.color = normalColor;
        }
    }
}