using TMPro;
using UnityEngine;

namespace _Project.Runtime.Core.UI.HUD
{
    public class UIHint : MonoBehaviour
    {
        [SerializeField] private GameObject root;
        [SerializeField] private TextMeshProUGUI text;

        private void Awake()
        {
            Hide();
        }

        public void Show(string message)
        {
            text.text = message;
            root.SetActive(true);
        }

        public void Hide()
        {
            root.SetActive(false);
        }
    }
}