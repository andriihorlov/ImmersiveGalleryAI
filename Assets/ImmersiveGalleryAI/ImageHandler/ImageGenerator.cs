using System.Threading.Tasks;
using ImmersiveGalleryAI.Keyboard;
using ImmersiveGalleryAI.Web;
using Oculus.Voice;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ImmersiveGalleryAI.ImageHandler
{
    public class ImageGenerator : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _requestImageButton;
        [SerializeField] private Image _resultedImage;
        [SerializeField] private Loader.Loader _loadingLabel;
        [SerializeField] private Button _voiceButton;

#region Physical buttons

        // [Header("Physical buttons")]
        // [SerializeField]
        // private TriggerEventReceiver _inputFieldEventReceiver;
        //
        // [SerializeField] private TriggerEventReceiver _generateButtonEventReceiver;
#endregion

        [Inject] private IWebManager _webManager;
        [Inject] private IKeyboard _keyboard;

        private bool _isMicEnabled;

        private void Awake()
        {
            _keyboard.Target = _inputField;
        }

        private void OnEnable()
        {
            _requestImageButton.onClick.AddListener(GenerateImageEventHandler);
            _inputField.onSelect.AddListener(InputFieldSelectedEventHandler);
            _voiceButton.onClick.AddListener(VoiceClickedEventHandler);
            
            _appVoiceExperience.events.OnPartialTranscription.AddListener(OnRequestTranscript);
            _appVoiceExperience.events.OnFullTranscription.AddListener(OnRequestTranscript);
            _appVoiceExperience.events.OnStoppedListeningDueToDeactivation.AddListener(OnStoppedListeningDueToDeactivation);

#region Physical button

            // _inputFieldEventReceiver.TriggerEnter += InputFieldEventHandler;
            // _generateButtonEventReceiver.TriggerEnter += GenerateImageTriggerEventHandler;

#endregion
        }

        private void OnDisable()
        {
            _requestImageButton.onClick.RemoveListener(GenerateImageEventHandler);
            _inputField.onSelect.RemoveListener(InputFieldSelectedEventHandler);
            _voiceButton.onClick.RemoveListener(VoiceClickedEventHandler);
            
            _appVoiceExperience.events.OnPartialTranscription.RemoveListener(OnRequestTranscript);
            _appVoiceExperience.events.OnFullTranscription.RemoveListener(OnRequestTranscript);
            _appVoiceExperience.events.OnStoppedListeningDueToDeactivation.RemoveListener(OnStoppedListeningDueToDeactivation);

#region Physical button

            // _inputFieldEventReceiver.TriggerEnter -= InputFieldEventHandler;
            // _generateButtonEventReceiver.TriggerEnter -= GenerateImageTriggerEventHandler;

#endregion
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
            _keyboard.SetActive(isActive);
        }

        private async void GenerateImageEventHandler()
        {
            _resultedImage.sprite = null;
            _requestImageButton.enabled = false;
            _inputField.enabled = false;
            _loadingLabel.SetActive(true);

            Task<Sprite> resultedSprite = _webManager.GenerateImageEventHandler(_inputField.text);
            await resultedSprite;

            _resultedImage.sprite = resultedSprite.Result;
            _requestImageButton.enabled = true;
            _inputField.enabled = true;
            _loadingLabel.SetActive(false);
        }

        private void InputFieldSelectedEventHandler(string arg0)
        {
            if (_keyboard.IsActive)
            {
                return;
            }

            _keyboard.SetActive(true);
        }

        private const string EnableMic = "Voice";
        private const string DisableMic = "Stop";
        [Space]
        [SerializeField] private TextMeshProUGUI _voiceButtonText;
        [SerializeField] private AppVoiceExperience _appVoiceExperience;
        

#region Voice recognition

        private void VoiceClickedEventHandler()
        {
            ToggleMicButton(!_isMicEnabled);
            
            if (_isMicEnabled)
            {
                _appVoiceExperience.Activate();
            }
            else
            {
                ToggleMicButton(false);
            }
        }


        private void ToggleMicButton(bool isActive)
        {
            _isMicEnabled = isActive;
            _voiceButtonText.text = _isMicEnabled ? DisableMic : EnableMic;
        }
        
        private void OnStoppedListeningDueToDeactivation()
        {
            ToggleMicButton(false);
        }

        private void OnRequestTranscript(string transcript)
        {
            _inputField.text = transcript;
            ToggleMicButton(false);
        }

#endregion

#region Physical button

        // private void InputFieldEventHandler(Collider otherCollider)
        // {
        //     if (!otherCollider.CompareTag("Hand"))
        //     {
        //         return;
        //     }
        //
        //     if (!_keyboard.IsActive)
        //     {
        //         _keyboard.SetActive(true);
        //     }
        // }
        //
        // private void GenerateImageTriggerEventHandler(Collider otherCollider)
        // {
        //     if (!otherCollider.CompareTag("Hand"))
        //     {
        //         return;
        //     }
        //     
        //     GenerateImageEventHandler();
        // }

#endregion
    }
}