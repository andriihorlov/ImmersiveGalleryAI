using System;
using System.Threading.Tasks;
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
        private bool _isPanelOpened;

        [Inject] private IWebManager _webManager;
        [Inject] private IImageDataManager _imageDataManager;
        [Inject] private IBackend _backend;
        [Inject] private ICredits _credits;

        public int WallId => _wallId;
        public ControlPanel ControlPanel => _controlPanel;
        private Material CurrentMaterial => _currentMaterial ??= _pictureMesh.material;

        private void Awake()
        {
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
            
            _credits.UpgradeBalanceEvent += UpgradeBalanceEventHandler;
            _credits.NoCreditsLeftEvent += NoCreditsLeftEventHandler;
        }

        private void OnDisable()
        {
            _controlPanel.GenerateImageClicked -= GenerateImageEventHandler;
            _controlPanel.SaveClicked -= SaveImageLocallyEventHandler;
            _controlPanel.CancelClicked -= CancelEventHandler;
            
            _lowerPanel.EditButtonEvent -= OpenPanelEventHandler;
            _lowerPanel.SaveButtonEvent -= SaveImageBackendEventHandler;
            _lowerPanel.DeleteButtonEvent -= DeleteClickedEventHandler;
            
            _credits.UpgradeBalanceEvent -= UpgradeBalanceEventHandler;
            _credits.NoCreditsLeftEvent -= NoCreditsLeftEventHandler;
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
            CurrentMaterial.mainTexture = null;
            _loadingLabel.SetActive(true);
            _controlPanel.ToggleButtons(false);
            Task<Texture2D> resultedTexture = _webManager.GenerateImageEventHandler(_controlPanel.InputField.text);
            _credits.SpendCredit();
            
            await resultedTexture;
            _currentTexture = resultedTexture.Result;
            CurrentMaterial.mainTexture = _currentTexture;
            _controlPanel.ToggleButtons(true);
            _loadingLabel.SetActive(false);
            _controlPanel.SetRegenerateButtons(true);
            
            if (_currentTexture == null)
            {
                _additionalInformation.SetActive(true);
                return;
            }

            byte[] bytes = _currentTexture.EncodeToJPG();
            _currentImage = new ImageData.ImageData {FileContent = bytes, WallId = _wallId, Description = _controlPanel.InputField.text};
            _additionalInformation.SetActive(false);
            _lowerPanel.SetActiveSaveDeleteButtons(true);
        }
        
        private void SaveImageLocallyEventHandler()
        {
           SaveImage(false);
        }

        private void CancelEventHandler()
        {
            HideControlPanel();
        }

        private void UpgradeBalanceEventHandler()
        {
            // sent email to admin with request possibility
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
            _imageDataManager.DeleteImage(_currentImage);
            _currentImage = null;
            _currentTexture = null;
            //_resultedImage.sprite = null;
            CurrentMaterial.mainTexture = null;
        }

        [ContextMenu("Open | Hide panel")]
        private void OpenPanelEventHandler()
        {
            OpenedPanel?.Invoke(this);
            _isPanelOpened = !_isPanelOpened;
            LowerPanelSetActive(!_isPanelOpened);
            ControlPanelSetActive(_isPanelOpened);
            
        }

        [ContextMenu("Save image")]
        private async void SaveImage(bool isBackendSent)
        {
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
    }
}