using System.Threading.Tasks;
using ImmersiveGalleryAI.Keyboard;
using ImmersiveGalleryAI.Loader;
using ImmersiveGalleryAI.VoiceRecognition;
using ImmersiveGalleryAI.Web;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ImmersiveGalleryAI.ImageHandler
{
    public class ImageGenerator : MonoBehaviour
    {
        [SerializeField] private Image _resultedImage;
        [SerializeField] private LoaderHandler _loadingLabel;
        [SerializeField] private ControlPanel _controlPanel;

        private bool _isMicEnabled;
        
#region Physical buttons

        // [Header("Physical buttons")]
        // [SerializeField]
        // private TriggerEventReceiver _inputFieldEventReceiver;
        //
        // [SerializeField] private TriggerEventReceiver _generateButtonEventReceiver;
#endregion
        
        [Inject] private IWebManager _webManager;
        [Inject] private IKeyboard _keyboard;
        [Inject] private IVoiceHandler _voiceHandler;


        private void Awake()
        {
            _keyboard.Target = _controlPanel.InputField;
        }

        private void OnEnable()
        {
            _controlPanel.GenerateImageClicked += GenerateImageEventHandler;
            _controlPanel.VoiceClicked += VoiceClickedEventHandler;
            _controlPanel.InputFieldSelected += InputFieldSelectedEventHandler;

            _voiceHandler.TranscriptionDoneEvent += OnRequestTranscript;
            _voiceHandler.StoppedListeningEvent += OnStoppedListeningDueToDeactivation;

#region Physical button

            // _inputFieldEventReceiver.TriggerEnter += InputFieldEventHandler;
            // _generateButtonEventReceiver.TriggerEnter += GenerateImageTriggerEventHandler;

#endregion
        }

        private void OnDisable()
        {
            _controlPanel.GenerateImageClicked -= GenerateImageEventHandler;
            _controlPanel.VoiceClicked -= VoiceClickedEventHandler;
            _controlPanel.InputFieldSelected -= InputFieldSelectedEventHandler;
            
            _voiceHandler.TranscriptionDoneEvent -= OnRequestTranscript;
            _voiceHandler.StoppedListeningEvent -= OnStoppedListeningDueToDeactivation;

#region Physical button

            // _inputFieldEventReceiver.TriggerEnter -= InputFieldEventHandler;
            // _generateButtonEventReceiver.TriggerEnter -= GenerateImageTriggerEventHandler;

#endregion
        }

        private async void GenerateImageEventHandler()
        {
            _resultedImage.sprite = null;
            
            _loadingLabel.SetActive(true);
            _controlPanel.ToggleButtons(false);
            Task<Sprite> resultedSprite = _webManager.GenerateImageEventHandler(_controlPanel.InputField.text);
            await resultedSprite;

            _resultedImage.sprite = resultedSprite.Result;
            _controlPanel.ToggleButtons(true);
            _loadingLabel.SetActive(false);
        }

        private void InputFieldSelectedEventHandler()
        {
            if (_keyboard.IsActive)
            {
                return;
            }

            _keyboard.SetActive(true);
        }


#region Voice recognition

        private void VoiceClickedEventHandler()
        {
            ToggleMicButton(!_isMicEnabled);
            
            if (_isMicEnabled)
            {
                _voiceHandler.Activate();
            }
            else
            {
                ToggleMicButton(false);
            }
        }

        private void OnStoppedListeningDueToDeactivation()
        {
            ToggleMicButton(false);
        }

        private void OnRequestTranscript(string transcript)
        {
            _controlPanel.InputField.text = transcript;
            ToggleMicButton(false);
        }

        private void ToggleMicButton(bool isActive)
        {
            _isMicEnabled = isActive;
            _controlPanel.ToggleMicText(isActive);
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