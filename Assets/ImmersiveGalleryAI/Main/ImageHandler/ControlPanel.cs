using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ImmersiveGalleryAI.Main.ImageHandler
{
    public class ControlPanel : MonoBehaviour
    {
        public event Action GenerateImageClicked;
        public event Action InputFieldSelected;
        public event Action VoiceClicked;
        public event Action SaveClicked;
        public event Action CancelClicked;

        private const string EnableMic = "Voice";
        private const string DisableMic = "Stop";

        private readonly Vector3 DefaultScale = Vector3.one;

        private const float ShowAnimationDuration = 1.5f;
        private const float HideAnimationDuration = 0.5f;
        private const float DefaultScaleHeight = 1f;
        private const float HideScaleHeight = 0f;

        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _generateButton;
        [SerializeField] private Button _voiceButton;
        [Space] [SerializeField] private TextMeshProUGUI _voiceButtonText;

        [Header("Generated buttons")]
        [SerializeField] private GameObject _generatedButtons;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _regenerateButton;

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
            _generateButton.onClick.AddListener(GenerateImageEventHandler);
            _inputField.onSelect.AddListener(InputFieldSelectedEventHandler);
            _voiceButton.onClick.AddListener(VoiceClickedEventHandler);

            _cancelButton.onClick.AddListener(CancelClickedEventHandler);
            _saveButton.onClick.AddListener(SaveClickedEventHandler);
            _regenerateButton.onClick.AddListener(RegenerateClickedEventHandler);
        }

        private void OnDisable()
        {
            _generateButton.onClick.RemoveListener(GenerateImageEventHandler);
            _inputField.onSelect.RemoveListener(InputFieldSelectedEventHandler);
            _voiceButton.onClick.RemoveListener(VoiceClickedEventHandler);

            _cancelButton.onClick.RemoveListener(CancelClickedEventHandler);
            _saveButton.onClick.RemoveListener(SaveClickedEventHandler);
            _regenerateButton.onClick.RemoveListener(RegenerateClickedEventHandler);
        }

        public void ToggleButtons(bool isActive)
        {
            _generateButton.enabled = isActive;
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
                SetRegenerateButtons(false);
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
        
        public void SetRegenerateButtons(bool isActive)
        {
            _generatedButtons.SetActive(isActive);
            _generateButton.gameObject.SetActive(!isActive);
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

        // private void ShareClickedEventHandler()
        // {
        //     ShareClicked?.Invoke();
        // }
        
        
        private void CancelClickedEventHandler()
        {
            CancelClicked?.Invoke();
        }
        
        private void SaveClickedEventHandler()
        {
            SaveClicked?.Invoke();
        }

        private void RegenerateClickedEventHandler()
        {
            GenerateImageClicked?.Invoke();
        }


    }
}