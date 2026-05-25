using System.Collections;
using UnityEngine;
using Zenject;

namespace _Project.Runtime.Core.UI.HUD
{
    public class LowTimeOverlayView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float fadeSpeed = 1f;

        [SerializeField] private float warningTime = 60f;
        [SerializeField] private float dangerTime = 30f;
        [SerializeField] private float criticalTime = 15f;

        [Inject] private IHealthObservable _health;

        private float _baseAlpha;
        private float _currentAlpha;
        private float _hitAlpha;

        private Coroutine _hitRoutine;

        private void OnEnable()
        {
            _health.OnHealthChanged += OnHealthChanged;
            _health.OnHit += PlayHitFlash;
        }

        private void OnDisable()
        {
            _health.OnHealthChanged -= OnHealthChanged;
            _health.OnHit -= PlayHitFlash;
        }

        private void OnHealthChanged(float time)
        {
            if (time <= criticalTime)
                _baseAlpha = 1f;
            else if (time <= dangerTime)
                _baseAlpha = 0.8f;
            else if (time <= warningTime)
                _baseAlpha = 0.6f;
            else
                _baseAlpha = 0f;
        }

        private void PlayHitFlash()
        {
            if (_hitRoutine != null)
                StopCoroutine(_hitRoutine);

            _hitRoutine = StartCoroutine(HitFlashRoutine());
        }

        private IEnumerator HitFlashRoutine()
        {
            var duration = 0.25f;
            var hold = 0.05f;

            _hitAlpha = 1f;

            yield return new WaitForSeconds(hold);

            var t = 0f;

            while (t < duration)
            {
                t += Time.deltaTime;

                var k = t / duration;
                k *= k;

                _hitAlpha = Mathf.Lerp(1f, 0f, k);

                yield return null;
            }

            _hitAlpha = 0f;
        }

        private void Update()
        {
            var target = _baseAlpha;

            target += _hitAlpha * 1.5f;

            target = Mathf.Clamp01(target);

            _currentAlpha = Mathf.Lerp(
                _currentAlpha,
                target,
                Time.deltaTime * fadeSpeed
            );

            canvasGroup.alpha = _currentAlpha;
        }
    }
}