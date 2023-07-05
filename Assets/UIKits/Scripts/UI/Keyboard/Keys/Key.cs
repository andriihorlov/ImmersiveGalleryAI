using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#if UIKIT_TMP
using TMPro;
#endif
namespace VRUiKits.Utils
{
    public class Key : MonoBehaviour
    {
        // The event which other objects can subscribe to
        // Uses the function defined above as its type
        public event OnKeyClickedHandler OnKeyClicked;
        public delegate void OnKeyClickedHandler(string key);
        
        private const string HandTagName = "Hand";
        private const float AnimationTime = 0.5f;
        private const float ButtonPressedPoseZ = 50f;

#if UIKIT_TMP
        protected TextMeshProUGUI key;
#else
        protected TextMeshProUGUI key;
#endif

        private Button _button;
        private Transform _currentTransform; 

        public bool IsPhysicalButton { get; set; }

        public virtual void Awake()
        {
            if (key == null)
            {
                key = GetComponentInChildren<TextMeshProUGUI>();
            }

            InitButton();
            _currentTransform = transform;
        }

        public virtual void CapsLock(bool isUppercase)
        {
        }

        public virtual void ShiftKey()
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!IsPhysicalButton)
            {
                return;
            }

            if (!other.CompareTag(HandTagName))
            {
                return;
            }

            OnKeyClickedEventHandler();
            ButtonPressedAnimation(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!IsPhysicalButton)
            {
                return;
            }

            if (!other.CompareTag(HandTagName))
            {
                return;
            }
            
            ButtonPressedAnimation(false);
        }

        private void OnDestroy()
        {
            if (IsPhysicalButton)
            {
                return;
            }

            _button.onClick.RemoveListener(OnKeyClickedEventHandler);
        }

        private void InitButton()
        {
            if (IsPhysicalButton)
            {
                return;
            }

            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnKeyClickedEventHandler);
        }

        private void OnKeyClickedEventHandler()
        {
            OnKeyClicked?.Invoke(key.text);
        }

        private void ButtonPressedAnimation(bool isPressed)
        {
            DOTween.Kill(_currentTransform);
            float targetPose = isPressed ? ButtonPressedPoseZ : 0f;
            _currentTransform.DOMoveZ(targetPose, AnimationTime).SetId(_currentTransform);
        }
    }
}