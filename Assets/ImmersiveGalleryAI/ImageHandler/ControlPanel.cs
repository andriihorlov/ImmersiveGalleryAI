using System;
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

        private const string EnableMic = "Voice";
        private const string DisableMic = "Stop";

        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _requestImageButton;
        [SerializeField] private Button _voiceButton;
        [Space] [SerializeField] private TextMeshProUGUI _voiceButtonText;

        private bool _isMicEnabled;
        public TMP_InputField InputField => _inputField;

        private void OnEnable()
        {
            _requestImageButton.onClick.AddListener(GenerateImageEventHandler);
            _inputField.onSelect.AddListener(InputFieldSelectedEventHandler);
            _voiceButton.onClick.AddListener(VoiceClickedEventHandler);
        }

        private void OnDisable()
        {
            _requestImageButton.onClick.RemoveListener(GenerateImageEventHandler);
            _inputField.onSelect.RemoveListener(InputFieldSelectedEventHandler);
            _voiceButton.onClick.RemoveListener(VoiceClickedEventHandler);
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
    }
}