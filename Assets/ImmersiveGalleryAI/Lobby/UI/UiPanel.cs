using DG.Tweening;
using UnityEngine;

namespace ImmersiveGalleryAI.Lobby.UI
{
    public class UiPanel : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        public Tween SetActive(bool isActive, float duration = -1)
        {
            float targetFade = isActive ? 1f : 0f;
            float fadeDuration = duration;
            if (fadeDuration < 0)
            {
                fadeDuration = isActive ? 1.5f : 0.5f;
            }

            if (isActive)
            {
                _canvasGroup.gameObject.SetActive(true);
            }

            return _canvasGroup.DOFade(targetFade, fadeDuration).OnComplete(() =>
            {
                if (!isActive)
                {
                    _canvasGroup.gameObject.SetActive(false);
                }
            });
        }

        public void SetActiveImmediate(bool isActive)
        {
            _canvasGroup.alpha = isActive ? 1f : 0f;
            _canvasGroup.gameObject.SetActive(isActive);
        }

        public void SetCanvasInteractables(bool isActive)
        {
            _canvasGroup.interactable = isActive;
            _canvasGroup.blocksRaycasts = isActive;
        }
    }
}