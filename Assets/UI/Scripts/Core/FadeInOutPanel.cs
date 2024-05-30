using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CustomUI
{
    [RequireComponent(typeof(Image))]
    public class FadeInOutPanel : MonoBehaviour, IFadeInOut
    {
        private bool _isFadeInProgress;
        [SerializeField] private Image _image;

        public async Task FadeIn(float durationSec)
        {
            await Fade(0f, 1f, durationSec);
        }

        public async Task FadeOut(float durationSec)
        {
            await Fade(1f, 0f, durationSec);
        }

        private async Task Fade(float startAlpha, float targetAlpha, float durationSec)
        {
            if (!_isFadeInProgress)
            {
                _image.enabled = true;
                _isFadeInProgress = true;
                float elapsedTime = 0f;

                while ((elapsedTime < durationSec) && !_image.IsDestroyed())
                {
                    elapsedTime += Time.deltaTime;
                    SetImageAlpha(_image, Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / durationSec));
                    await Task.Yield();
                }

                if (!_image.IsDestroyed() && _image.color.a == 0f)
                {
                    _image.enabled = false;
                }
                _isFadeInProgress = false;
            }
        }

        private void SetImageAlpha(Image image, float alpha)
        {
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;
        }
    }
}