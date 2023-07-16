using System;
using DG.Tweening;
using ImmersiveGalleryAI.Utilities;
using UnityEngine;

namespace ImmersiveGalleryAI.ImageHandler
{
    public class SpotHandler : MonoBehaviour
    {
        public event Action SpotReachedEvent;
        public event Action SpotOutEvent;

        [SerializeField] private TriggerEventReceiver _spotTriggerReceiver;

        private const Ease AnimationEase = Ease.OutCirc;
        private const float DefaultPositionY = 0f;
        private const float DownPositionY = -1.9f;

        private const float UpAnimationTime = 0.25f;
        private const float DownAnimationTime = 1f;

        private const string PlayerTag = "Player";

        private Transform _currentTransform;

        private void Awake()
        {
            _currentTransform = transform;
        }

        private void OnEnable()
        {
            _spotTriggerReceiver.TriggerEnter += TriggerEnterEventReceiver;
            _spotTriggerReceiver.TriggerExit += TriggerExitEventReceiver;
        }

        private void OnDisable()
        {
            _spotTriggerReceiver.TriggerEnter -= TriggerEnterEventReceiver;
            _spotTriggerReceiver.TriggerExit -= TriggerExitEventReceiver;
        }

        private void TriggerEnterEventReceiver(Collider other)
        {
            if (!other.CompareTag(PlayerTag))
            {
                return;
            }

            SpotReachedEvent?.Invoke();
            ButtonAnimation(isDown: true);
        }

        private void TriggerExitEventReceiver(Collider other)
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