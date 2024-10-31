using System;
using System.Threading.Tasks;
using ImmersiveGalleryAI.Common.AudioSystem;
using ImmersiveGalleryAI.Common.Backend;
using ImmersiveGalleryAI.Common.Loader;
using ImmersiveGalleryAI.Common.Web;
using ImmersiveGalleryAI.Main.Credits;
using ImmersiveGalleryAI.Main.ImageData;
using ImmersiveGalleryAI.Main.UI;
using UnityEngine;
using Zenject;

namespace ImmersiveGalleryAI.Main.ImageHandler
{
    public class WallImage : MonoBehaviour
    {
        public event Action<WallImage> OpenedPanel;

        [SerializeField] private int _wallId;
        [SerializeField] private LoaderHandler _loadingLabel;
        [SerializeField] private ControlPanel _controlPanel;
        [SerializeField] private LowerPanel _lowerPanel;
        [SerializeField] private UpperPanel _upperPanel;
        [SerializeField] private MeshRenderer _pictureMesh;
        [SerializeField] private AdditionalInformation _additionalInformation;

        private Material _currentMaterial;
        private ImageData.ImageData _currentImage;
        private Texture2D _currentTexture;
        private Texture _defaultTexture;
        private bool _isPanelOpened;

        [Inject] private IWebManager _webManager;
        [Inject] private IImageDataManager _imageDataManager;
        [Inject] private IBackend _backend;
        [Inject] private ICredits _credits;
        [Inject] private IAudioSystem _audioSystem;

        public int WallId => _wallId;
        public ControlPanel ControlPanel => _controlPanel;
        private Material CurrentMaterial => _currentMaterial ??= _pictureMesh.material;

        private void Awake()
        {
            _defaultTexture = CurrentMaterial.mainTexture;
            _additionalInformation.SetActive(true, AdditionalInfoType.Default);
            _lowerPanel.SetActiveSaveDeleteButtons(false);
        }

        private void OnEnable()
        {
            _controlPanel.GenerateImageClicked += GenerateImageEventHandler;
            _controlPanel.SaveClicked += SaveImageLocallyEventHandler;
            _controlPanel.CancelClicked += CancelEventHandler;
            
            _lowerPanel.EditButtonEvent += OpenPanelEventHandler;
            _lowerPanel.SaveButtonEvent += SaveImageBackendEventHandler;
            _lowerPanel.DeleteButtonEvent += DeleteClickedEventHandler;
            
            _credits.NoCreditsLeftEvent += NoCreditsLeftEventHandler;
            _upperPanel.RequestUpgradeBalance += RequestUpgradeBalanceEventHandler;
        }

        private void OnDisable()
        {
            _controlPanel.GenerateImageClicked -= GenerateImageEventHandler;
            _controlPanel.SaveClicked -= SaveImageLocallyEventHandler;
            _controlPanel.CancelClicked -= CancelEventHandler;
            
            _lowerPanel.EditButtonEvent -= OpenPanelEventHandler;
            _lowerPanel.SaveButtonEvent -= SaveImageBackendEventHandler;
            _lowerPanel.DeleteButtonEvent -= DeleteClickedEventHandler;
            
            _credits.NoCreditsLeftEvent -= NoCreditsLeftEventHandler;
            _upperPanel.RequestUpgradeBalance -= RequestUpgradeBalanceEventHandler;
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void LoadPreviousImage(ImageData.ImageData imageData)
        {
            _currentImage = imageData;
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(imageData.FileContent);
            texture.Apply();
            CurrentMaterial.mainTexture = texture;
            
            _additionalInformation.SetActive(false);
            _lowerPanel.SetActiveSaveDeleteButtons(true);
            _controlPanel.InitPreviousText(imageData.Description);
        }

        public void HideControlPanel()
        {
            ControlPanelSetActive(false);
            LowerPanelSetActive(true);
            _isPanelOpened = false;
        }
        
        [ContextMenu("Save image on backend")]
        private void SaveImageBackendEventHandler()
        {
            SaveImage(true);
        }

        private void ControlPanelSetActive(bool isActive)
        {
            _controlPanel.SetActive(isActive);
        }
        
        private void LowerPanelSetActive(bool isActive)
        {
            _lowerPanel.SetActive(isActive);
        }

        private async void GenerateImageEventHandler()
        {
            if (!_credits.IsOwnCredits && _credits.GetCreditsBalance() < 1)
            {
                NoCreditsLeftEventHandler();
                return;
            }
            
            _audioSystem.PlayClickSfx();
            _loadingLabel.SetActive(true);
            _controlPanel.ToggleButtons(false);
            Task<Texture2D> resultedTexture = _webManager.GenerateImageEventHandler(_controlPanel.InputField.text);
            _credits.SpendCredit();
            _additionalInformation.SetActive(false);
            await resultedTexture;
            
            _controlPanel.ToggleButtons(true);
            _loadingLabel.SetActive(false);
            _controlPanel.SetRegenerateButtons(true);
            _currentTexture = resultedTexture.Result;
            
            if (_currentTexture == null)
            {
                _additionalInformation.SetActive(true, _webManager.ErrorMessage);
                return;
            }
            
            CurrentMaterial.mainTexture = _currentTexture;
            byte[] bytes = _currentTexture.EncodeToJPG();
            _currentImage = new ImageData.ImageData {FileContent = bytes, WallId = _wallId, Description = _controlPanel.InputField.text};
            _lowerPanel.SetActiveSaveDeleteButtons(true);
        }
        
        private void SaveImageLocallyEventHandler()
        {   
            _audioSystem.PlayClickSfx();
            SaveImage(false);
        }

        private void CancelEventHandler()
        {
            _audioSystem.PlayClickSfx();
            HideControlPanel();
        }

        private void RequestUpgradeBalanceEventHandler()
        {
            _audioSystem.PlayClickSfx();
            _credits.RequestUpgradeBalance();
        }
        
        private void NoCreditsLeftEventHandler()
        {
            if (_currentImage == null)
            {
                _additionalInformation.SetActive(true, AdditionalInfoType.NoCredits);
            }
        }

        [ContextMenu("Delete currentImage")]
        private void DeleteClickedEventHandler()
        {
            _audioSystem.PlayClickSfx();
            _imageDataManager.DeleteImage(_currentImage);
            _currentImage = null;
            _currentTexture = null;
            CurrentMaterial.mainTexture = _defaultTexture;
        }

        [ContextMenu("Open | Hide panel")]
        private void OpenPanelEventHandler()
        {
            _audioSystem.PlayClickSfx();
            OpenedPanel?.Invoke(this);
            _isPanelOpened = !_isPanelOpened;
            LowerPanelSetActive(!_isPanelOpened);
            ControlPanelSetActive(_isPanelOpened);
        }

        [ContextMenu("Save image")]
        private void SaveImageEditor()
        {
            SaveImage(true);
        }
        
        private async void SaveImage(bool isBackendSent)
        {
            _audioSystem.PlayClickSfx();
            _imageDataManager.SaveImage(_currentImage);
            if (isBackendSent)
            {
                _loadingLabel.SetActive(true);
                await _backend.UploadImageData(_currentImage);
                _loadingLabel.SetActive(false);
            }
            HideControlPanel();
        }

        [ContextMenu("Generate image")]
        private void GenerateImage()
        {
            GenerateImageEventHandler();
        }

        public void HideUpperPanel()
        {
            _upperPanel.SetActive(false);
        }

        public void UpdateCreditsBalance(int newCredits)
        {
            _upperPanel.UpdateBalance(newCredits);
        }
    }
}