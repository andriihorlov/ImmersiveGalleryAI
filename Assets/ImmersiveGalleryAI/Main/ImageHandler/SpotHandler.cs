using System;
using DG.Tweening;
using ImmersiveGalleryAI.Common.Utilities;
using UnityEngine;

namespace ImmersiveGalleryAI.Main.ImageHandler
{
    public class SpotHandler : MonoBehaviour
    {
        public event Action SpotReachedEvent;
        public event Action SpotOutEvent;

        [SerializeField] private TriggerEventReceiver _spotTriggerReceiver;
        [SerializeField] private Transform _spotTransform;

        private const Ease AnimationEase = Ease.OutCirc;
        private const float DefaultPositionY = 0f;
        private const float DownPositionY = -0.09f;

        private const float UpAnimationTime = 0.25f;
        private const float DownAnimationTime = 1f;

        private const string PlayerTag = "Player";

        private bool _isPlayerOnSpot;

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
            if (!other.CompareTag(PlayerTag) || _isPlayerOnSpot)
            {
                return;
            }

            _isPlayerOnSpot = true;
            SpotReachedEvent?.Invoke();
            ButtonAnimation(isDown: true);
            Debug.Log($"Player entered");
        }

        private void TriggerExitEventReceiver(Collider other)
        {
            if (!other.CompareTag(PlayerTag) || !_isPlayerOnSpot)
            {
                return;
            }
            
            _isPlayerOnSpot = false;
            SpotOutEvent?.Invoke();
            ButtonAnimation(isDown: false);
            Debug.Log($"Player exit");
        }
        
        private void ButtonAnimation(bool isDown)
        {
            DOTween.Kill(_spotTransform);
            _spotTransform
                .DOLocalMoveY(isDown ? DownPositionY : DefaultPositionY, isDown ? DownAnimationTime : UpAnimationTime)
                .SetEase(AnimationEase)
                .SetId(_spotTransform);
        }
    }
}