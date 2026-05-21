using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.Runtime.Menu.UI
{
    public class UISelectorMover : MonoBehaviour
    {
        [SerializeField] private RectTransform selector;
        [SerializeField] private RectTransform positionRoot;
        [SerializeField] private float smoothSpeed = 15f;
        [SerializeField] private float padding = 20f; 

        private RectTransform _target;
        private Canvas _canvas;

        private void Awake()
        {
            if (!positionRoot)
                positionRoot = selector.parent as RectTransform;

            _canvas = selector.GetComponentInParent<Canvas>();
        }

        private void Update()
        {
            var selected = EventSystem.current?.currentSelectedGameObject;

            if (!selected || !selected.TryGetComponent(out RectTransform target))
            {
                _target = null;
                if (selector.gameObject.activeSelf)
                    selector.gameObject.SetActive(false);
                return;
            }

            _target = target;
            if (!selector.gameObject.activeSelf)
                selector.gameObject.SetActive(true);

            var targetAnchored = WorldToAnchoredInRoot(GetLeftEdgeWithPadding(_target));
            selector.anchoredPosition = Vector2.Lerp(
                selector.anchoredPosition,
                targetAnchored,
                Time.unscaledDeltaTime * smoothSpeed
            );
        }

        private Vector3 GetLeftEdgeWithPadding(RectTransform target)
        {
            var rect = target.rect;
            var localPoint = new Vector3(rect.xMin - padding, rect.center.y, 0f);
            return target.TransformPoint(localPoint);
        }

        private Vector2 WorldToAnchoredInRoot(Vector3 worldPos)
        {
            var cam = _canvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null
                : _canvas.worldCamera;

            var screenPoint = RectTransformUtility.WorldToScreenPoint(cam, worldPos);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                positionRoot, screenPoint, cam, out var localPoint);

            return localPoint;
        }
    }
}