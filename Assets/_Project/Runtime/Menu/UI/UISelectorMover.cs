using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Runtime.Menu.UI
{
    public class UISelectorMover : MonoBehaviour
    {
        [SerializeField] private RectTransform selector;
        [SerializeField] private float smoothSpeed = 15f;
        [SerializeField] private float padding = 20f;

        private RectTransform _target;

        private void Update()
        {
            var selected = EventSystem.current.currentSelectedGameObject;

            if (selected &&
                selected.TryGetComponent<RectTransform>(out var target))
            {
                _target = target;

                if (!selector.gameObject.activeSelf)
                    selector.gameObject.SetActive(true);
            }
            else
            {
                _target = null;

                if (selector.gameObject.activeSelf)
                    selector.gameObject.SetActive(false);

                return;
            }

            var targetPos = GetLeftEdgeWithPadding(_target);

            selector.position = Vector3.Lerp(
                selector.position,
                targetPos,
                Time.unscaledDeltaTime * smoothSpeed
            );
        }

        private Vector3 GetLeftEdgeWithPadding(RectTransform target)
        {
            var corners = new Vector3[4];
            target.GetWorldCorners(corners);

            var leftEdge = (corners[0] + corners[1]) * 0.5f;

            var offset = -target.right * padding;

            return leftEdge + offset;
        }
    }
}