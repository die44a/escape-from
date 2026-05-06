using System.Threading.Tasks;
using UnityEngine;

namespace _Project.Global
{
    public class SceneFader : MonoBehaviour
    {
        [SerializeField] private CanvasGroup fadeCanvasGroup;
        [SerializeField] private float defaultDuration = 0.5f;

        private void Awake()
        {
            fadeCanvasGroup.blocksRaycasts = false;
        }
        
        public async Task FadeInAsync(float duration = -1f)
        {
            await DoFade(1f, 0f, duration > 0 ? duration : defaultDuration);
        }

        public async Task FadeOutAsync(float duration = -1f)
        {
            await DoFade(0f, 1f, duration > 0 ? duration : defaultDuration);
        }

        private async Task DoFade(float startAlpha, float endAlpha, float duration)
        {
            var elapsed = 0f;
            fadeCanvasGroup.alpha = startAlpha;

            if (endAlpha > 0) fadeCanvasGroup.blocksRaycasts = true;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
                await Task.Yield(); 
            }

            fadeCanvasGroup.alpha = endAlpha;

            if (endAlpha <= 0) fadeCanvasGroup.blocksRaycasts = false;
        }
    }
}