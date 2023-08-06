using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ImmersiveGalleryAI.ImageHandler
{
    public class ControlPanel : MonoBehaviour
    {
        public event Action GenerateImageClicked;
        public event Action InputFieldSelected;
        public event Action VoiceClicked;
        public event Action ShareClicked;
        public event Action DeleteClicked;

        private const string EnableMic = "Voice";
        private const string DisableMic = "Stop";

        private readonly Vector3 DefaultScale = Vector3.one;

        private const float ShowAnimationDuration = 1.5f;
        private const float HideAnimationDuration = 0.5f;
        private const float DefaultScaleHeight = 1f;
        private const float HideScaleHeight = 0f;

        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _requestImageButton;
        [SerializeField] private Button _voiceButton;
        [Space] [SerializeField] private Button _shareImageButton;
        [SerializeField] private Button _deleteImageButton;
        [Space] [SerializeField] private TextMeshProUGUI _voiceButtonText;

        private Transform _currentTransform;
        private bool _isMicEnabled;
        public TMP_InputField InputField => _inputField;

        private void Awake()
        {
            _currentTransform = transform;
            SetActive(false, true);
        }

        private void OnEnable()
        {
            _requestImageButton.onClick.AddListener(GenerateImageEventHandler);
            _inputField.onSelect.AddListener(InputFieldSelectedEventHandler);
            _voiceButton.onClick.AddListener(VoiceClickedEventHandler);

            _shareImageButton.onClick.AddListener(ShareClickedEventHandler);
            _deleteImageButton.onClick.AddListener(DeleteClickedEventHandler);
        }

        private void OnDisable()
        {
            _requestImageButton.onClick.RemoveListener(GenerateImageEventHandler);
            _inputField.onSelect.RemoveListener(InputFieldSelectedEventHandler);
            _voiceButton.onClick.RemoveListener(VoiceClickedEventHandler);

            _shareImageButton.onClick.RemoveListener(ShareClickedEventHandler);
            _deleteImageButton.onClick.RemoveListener(DeleteClickedEventHandler);
        }

        public void ToggleButtons(bool isActive)
        {
            _requestImageButton.enabled = isActive;
            _inputField.enabled = isActive;
        }

        public void ToggleMicText(bool isActive)
        {
            _voiceButtonText.text = isActive ? DisableMic : EnableMic;
        }

        public void SetActive(bool isActive, bool isImmediate = false)
        {
            Vector3 scale = DefaultScale;
            scale.y = isActive ? DefaultScaleHeight : HideScaleHeight;

            if (isImmediate)
            {
                gameObject.SetActive(isActive);
                _currentTransform.localScale = scale;
                return;
            }

            if (isActive)
            {
                gameObject.SetActive(true);
            }

            _currentTransform
                .DOScaleY(scale.y, isActive ? ShowAnimationDuration : HideAnimationDuration)
                .OnComplete(() =>
                {
                    if (!isActive)
                    {
                        _currentTransform.gameObject.SetActive(false);
                    }
                });
        }

        private void GenerateImageEventHandler()
        {
            GenerateImageClicked?.Invoke();
        }

        private void InputFieldSelectedEventHandler(string arg0)
        {
            InputFieldSelected?.Invoke();
        }

        private void VoiceClickedEventHandler()
        {
            VoiceClicked?.Invoke();
        }

        private void ToggleMicButton(bool isActive)
        {
            _isMicEnabled = isActive;
            _voiceButtonText.text = _isMicEnabled ? DisableMic : EnableMic;
        }

        private void ShareClickedEventHandler()
        {
            ShareClicked?.Invoke();
        }

        private void DeleteClickedEventHandler()
        {
            DeleteClicked?.Invoke();
        }
    }
}