using AiGalleryVR.Keyboard;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace AiGalleryVR.ImageHandler
{
    public class ImageController : MonoBehaviour
    {
        private const float FadeInAnimationDuration = 1f;
        private const float FadeOutAnimationDuration = 0.5f;

        [SerializeField] private CanvasGroup _imageCanvasGroup;
        [SerializeField] private ImageSpot _imageSpot;

        [Inject] private IKeyboard _keyboardManager;
        private bool _onSpot;

        private void Awake()
        {
            SetActiveImage(isActive: false, immediately: true);
        }

        private void OnEnable()
        {
            _imageSpot.SpotReachedEvent += SpotReachedEventHandler;
            _imageSpot.SpotOutEvent += SpotOutEventHandler;
            _keyboardManager.EnterPressedAction += EnterPressedEventHandler;
        }

        private void OnDisable()
        {
            _imageSpot.SpotReachedEvent -= SpotReachedEventHandler;
            _imageSpot.SpotOutEvent -= SpotOutEventHandler;
            _keyboardManager.EnterPressedAction -= EnterPressedEventHandler;
        }

        private void SpotOutEventHandler()
        {
            _onSpot = false;
            if (_keyboardManager.IsActive)
            {
                return;
            }
            
            SetActiveImage(false);
        }

        private void SpotReachedEventHandler()
        {
            if (_onSpot)
            {
                return;
            }
            
            SetActiveImage(true);
            _onSpot = true;
        }

        private void SetActiveImage(bool isActive, bool immediately = false)
        {
            float fadeValue = isActive ? 1f : 0f;
            if (immediately)
            {
                _imageCanvasGroup.alpha = fadeValue;
                return;
            }

            float duration = isActive ? FadeInAnimationDuration : FadeOutAnimationDuration; 
            _imageCanvasGroup.DOKill(_imageCanvasGroup);
            _imageCanvasGroup.DOFade(fadeValue, duration).SetId(_imageCanvasGroup);
        }
        
        private void EnterPressedEventHandler()
        {
            if (!_onSpot)
            {
                SetActiveImage(false);
            }
        }
    }
}