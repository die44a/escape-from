using System.Collections;
using UnityEngine;
using Zenject;
using _Project.Runtime.Player.Main;

namespace _Project.Runtime.Core.UI.HUD
{
    public class PlayerInteractBarView : MonoBehaviour
    {
        [SerializeField] private Transform maskTransform;
        [SerializeField] private SpriteRenderer fillRenderer;
        [SerializeField] private GameObject root;

        [SerializeField] private float fadeSpeed = 6f;

        [Inject] private IPlayerStatus _playerStatus;

        private Coroutine _fadeRoutine;
        private bool _isActive;
        private Color _baseColor;

        private Vector3 _baseScale;

        private void Awake()
        {
            _baseScale = maskTransform.localScale;
            _baseColor = fillRenderer.color;
        }

        private void OnEnable()
        {
            _playerStatus.OnStateChanged += OnStateChanged;
            OnStateChanged(_playerStatus.CurrentState);
        }

        private void OnDisable()
        {
            _playerStatus.OnStateChanged -= OnStateChanged;
        }

        private void OnStateChanged(PlayerState state)
        {
            var isInteracting = state == PlayerState.Interacting;

            if (isInteracting == _isActive)
                return;

            _isActive = isInteracting;

            if (_fadeRoutine != null)
                StopCoroutine(_fadeRoutine);

            _fadeRoutine = StartCoroutine(FadeRoutine(isInteracting));
        }

        private IEnumerator FadeRoutine(bool show)
        {
            root.SetActive(true);

            var targetAlpha = show ? 1f : 0f;
            var targetScale = show ? 1f : 0f;

            var alpha = fillRenderer.color.a;
            var scale = maskTransform.localScale.x / _baseScale.x;

            while (Mathf.Abs(alpha - targetAlpha) > 0.01f ||
                   Mathf.Abs(scale - targetScale) > 0.01f)
            {
                alpha = Mathf.Lerp(alpha, targetAlpha, Time.deltaTime * fadeSpeed);
                scale = Mathf.Lerp(scale, targetScale, Time.deltaTime * fadeSpeed);

                var color = _baseColor;
                color.a = alpha;
                fillRenderer.color = color;

                var newScale = _baseScale;
                newScale.x *= scale;

                maskTransform.localScale = newScale;

                yield return null;
            }

            if (!show)
            {
                fillRenderer.color = _baseColor;
                maskTransform.localScale = _baseScale;

                root.SetActive(false);
            }
        }
    }
}