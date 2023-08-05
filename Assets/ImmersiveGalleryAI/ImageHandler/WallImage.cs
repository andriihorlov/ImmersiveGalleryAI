using System.Threading.Tasks;
using ImmersiveGalleryAI.Data;
using ImmersiveGalleryAI.Keyboard;
using ImmersiveGalleryAI.Loader;
using ImmersiveGalleryAI.User;
using ImmersiveGalleryAI.VoiceRecognition;
using ImmersiveGalleryAI.Web;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ImmersiveGalleryAI.ImageHandler
{
    public class WallImage : MonoBehaviour
    {
        [SerializeField] private int _wallId;
        [SerializeField] private Image _resultedImage;
        [SerializeField] private LoaderHandler _loadingLabel;
        [SerializeField] private ControlPanel _controlPanel;
        [SerializeField] private Button _openPanelButton;

        private ImageData _currentImage;
        private Texture2D _currentTexture;
        private bool _isMicEnabled;

        [Inject] private IWebManager _webManager;
        [Inject] private IKeyboard _keyboard;
        [Inject] private IVoiceHandler _voiceHandler;
        [Inject] private IDataManager _dataManager;
        [Inject] private IUser _user;
        
        public int WallId => _wallId;

        private void Awake()
        {
            _keyboard.Target = _controlPanel.InputField;
        }

        private void OnEnable()
        {
            _controlPanel.GenerateImageClicked += GenerateImageEventHandler;
            _controlPanel.VoiceClicked += VoiceClickedEventHandler;
            _controlPanel.InputFieldSelected += InputFieldSelectedEventHandler;
            _controlPanel.ShareClicked += ShareClickedEventHandler;
            _controlPanel.DeleteClicked += DeleteClickedEventHandler;

            _voiceHandler.TranscriptionDoneEvent += RequestTranscriptEventHandler;
            _voiceHandler.StoppedListeningEvent += StoppedListeningEventHandler;

            _openPanelButton.onClick.AddListener(OpenPanelEventHandler);
        }

        private void OnDisable()
        {
            _controlPanel.GenerateImageClicked -= GenerateImageEventHandler;
            _controlPanel.VoiceClicked -= VoiceClickedEventHandler;
            _controlPanel.InputFieldSelected -= InputFieldSelectedEventHandler;
            _controlPanel.ShareClicked -= ShareClickedEventHandler;
            _controlPanel.DeleteClicked -= DeleteClickedEventHandler;

            _voiceHandler.TranscriptionDoneEvent -= RequestTranscriptEventHandler;
            _voiceHandler.StoppedListeningEvent -= StoppedListeningEventHandler;

            _openPanelButton.onClick.RemoveListener(OpenPanelEventHandler);
        }

        public void LoadPreviousImage(ImageData imageData)
        {
            _currentImage = imageData;
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(imageData.FileContent);
            texture.Apply();
            _resultedImage.sprite = CreateSprite(texture);
        }

        private async void GenerateImageEventHandler()
        {
            _resultedImage.sprite = null;
            _loadingLabel.SetActive(true);
            _controlPanel.ToggleButtons(false);
            Task<Texture2D> resultedTexture = _webManager.GenerateImageEventHandler(_controlPanel.InputField.text);
            await resultedTexture;
            _currentTexture = resultedTexture.Result;
            _resultedImage.sprite = CreateSprite(_currentTexture);
            _controlPanel.ToggleButtons(true);
            _loadingLabel.SetActive(false);
         
            if (_currentTexture != null)
            {
                _currentImage = new ImageData {FileContent = _currentTexture.EncodeToJPG(), WallId = _wallId};
                _dataManager.SaveImage(_currentImage);
            } 
        }

        private void InputFieldSelectedEventHandler()
        {
            if (_keyboard.IsActive)
            {
                return;
            }

            _keyboard.ChangePosition(_user.CameraRigTransform);
            _keyboard.SetActive(true);
        }

        private Sprite CreateSprite(Texture2D texture2D)
        {
            return Sprite.Create(texture2D, new Rect(0, 0, 256, 256), Vector2.zero);
        }

        private void ShareClickedEventHandler()
        {
            _dataManager.ShareImage(_currentImage);
        }
        
        private void DeleteClickedEventHandler()
        {
            _dataManager.DeleteImage(_currentImage);
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

        private void StoppedListeningEventHandler()
        {
            ToggleMicButton(false);
        }

        private void RequestTranscriptEventHandler(string transcript)
        {
            _controlPanel.InputField.text = transcript;
            ToggleMicButton(false);
        }

        private void ToggleMicButton(bool isActive)
        {
            _isMicEnabled = isActive;
            _controlPanel.ToggleMicText(isActive);
        }

        [ContextMenu("Open panel")]
        private void OpenPanelEventHandler()
        {
            _controlPanel.SetActive(true);
        }

        [ContextMenu("Hide panel")]
        private void HidePanelEventHandler()
        {
            _controlPanel.SetActive(false);
        }

        [ContextMenu("Generate image")]
        private void GenerateImage()
        {
            GenerateImageEventHandler();
        }

#endregion
    }
}