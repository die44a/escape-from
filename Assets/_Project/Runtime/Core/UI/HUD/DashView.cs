using System.Collections;
using UnityEngine;
using Zenject;

namespace _Project.Runtime.Core.UI.HUD
{
    public class DashSpriteBarView : MonoBehaviour
    {
        [SerializeField] private Transform maskTransform;
        [SerializeField] private SpriteRenderer fillRenderer;
        [SerializeField] private GameObject root;

        [SerializeField] private float maxWidth = 1f;

        [SerializeField] private float shakeStrength = 0.1f;
        [SerializeField] private float shakeDuration = 0.15f;

        [Inject] private IDashProvider _dashProvider;

        private bool _isVisible;

        private Vector3 _baseLocalPos;
        private Vector3 _baseScale;
        private bool _isFailFeedbackPlaying;

        private Coroutine _shakeRoutine;

        private void Awake()
        {
            _baseLocalPos = maskTransform.localPosition;
            _baseScale = maskTransform.localScale;
        }

        private void OnEnable()
        {
            _dashProvider.OnDashFailed += PlayFailFeedback;
        }

        private void OnDisable()
        {
            _dashProvider.OnDashFailed -= PlayFailFeedback;
        }

        private void Update()
        {
            var shouldBeVisible = !_dashProvider.IsDashReady;

            if (shouldBeVisible != _isVisible)
            {
                root.SetActive(shouldBeVisible);
                _isVisible = shouldBeVisible;

                if (!shouldBeVisible)
                {
                    SetWidth(0f);
                    return;
                }
            }

            if (!shouldBeVisible)
                return;

            SetWidth(_dashProvider.DashProgress);
        }

        private void SetWidth(float value)
        {
            maskTransform.localScale = new Vector3(
                _baseScale.x * value * maxWidth,
                _baseScale.y,
                _baseScale.z
            );
        }

        private void PlayFailFeedback()
        {
            if (_isFailFeedbackPlaying)
                return;

            if (_shakeRoutine != null)
                StopCoroutine(_shakeRoutine);

            _shakeRoutine = StartCoroutine(ShakeAndFlash());
        }

        private IEnumerator ShakeAndFlash()
        {
            _isFailFeedbackPlaying = true;

            var tr = maskTransform; 

            var originalPos = tr.localPosition;
            var originalColor = fillRenderer.color;

            var t = 0f;

            while (t < shakeDuration)
            {
                t += Time.deltaTime;

                var decay = 1f - (t / shakeDuration);
                var offsetX = Random.Range(-1f, 1f) * shakeStrength * decay;

                tr.localPosition = originalPos + new Vector3(offsetX, 0f, 0f);

                fillRenderer.color = Color.red;

                yield return null;
            }

            tr.localPosition = originalPos;
            fillRenderer.color = originalColor;

            _isFailFeedbackPlaying = false;
        }
    }
}