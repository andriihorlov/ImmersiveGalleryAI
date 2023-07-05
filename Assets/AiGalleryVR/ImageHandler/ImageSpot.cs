using System;
using DG.Tweening;
using UnityEngine;

namespace AiGalleryVR.ImageHandler
{
    public class ImageSpot : MonoBehaviour
    {
        public event Action SpotReachedEvent;
        public event Action SpotOutEvent;

        private const Ease AnimationEase = Ease.OutCirc;
        private const float DefaultPositionY = -1.5f;
        private const float DownPositionY = -1.57f;

        private const float UpAnimationTime = 0.25f;
        private const float DownAnimationTime = 1f;

        private const string PlayerTag = "Player";

        private Transform _currentTransform;

        private void Awake()
        {
            _currentTransform = transform;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(PlayerTag))
            {
                return;
            }

            SpotReachedEvent?.Invoke();
            ButtonAnimation(isDown: true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(PlayerTag))
            {
                return;
            }

            SpotOutEvent?.Invoke();
            ButtonAnimation(isDown: false);
        }
        
        private void ButtonAnimation(bool isDown)
        {
            DOTween.Kill(_currentTransform);
            _currentTransform
                .DOLocalMoveY(isDown ? DownPositionY : DefaultPositionY, isDown ? DownAnimationTime : UpAnimationTime)
                .SetEase(AnimationEase)
                .SetId(_currentTransform);
        }
    }
}