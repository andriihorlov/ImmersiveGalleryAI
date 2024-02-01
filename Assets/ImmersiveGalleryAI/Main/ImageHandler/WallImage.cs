using System;
using System.Threading.Tasks;
using ImmersiveGalleryAI.Common.Backend;
using ImmersiveGalleryAI.Common.Loader;
using ImmersiveGalleryAI.Common.Web;
using ImmersiveGalleryAI.Main.Credits;
using ImmersiveGalleryAI.Main.Data;
using UnityEngine;
using UnityEngine.UI;
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
        [SerializeField] private Button _openPanelButton;
        [SerializeField] private MeshRenderer _pictureMesh;

        private Material _currentMaterial;
        private ImageData _currentImage;
        private Texture2D _currentTexture;
        private bool _isPanelOpened;

        [Inject] private IWebManager _webManager;
        [Inject] private IImageDataManager _imageDataManager;
        [Inject] private IBackend _backend;
        [Inject] private ICredits _credits;

        public int WallId => _wallId;
        public ControlPanel ControlPanel => _controlPanel;
        private Material CurrentMaterial => _currentMaterial ??= _pictureMesh.material;

        private void OnEnable()
        {
            _controlPanel.GenerateImageClicked += GenerateImageEventHandler;
            // _controlPanel.ShareClicked += ShareClickedEventHandler;
            // _controlPanel.DeleteClicked += DeleteClickedEventHandler;
            //_openPanelButton.onClick.AddListener(OpenPanelEventHandler);

            _lowerPanel.EditButtonEvent += OpenPanelEventHandler;
            _lowerPanel.SaveButtonEvent += SaveButtonEventHandler;
            _lowerPanel.DeleteButtonEvent += DeleteClickedEventHandler;

            _credits.UpgradeBalanceEvent += UpgradeBalanceEventHandler;
        }

        private void OnDisable()
        {
            _controlPanel.GenerateImageClicked -= GenerateImageEventHandler;
            // _controlPanel.ShareClicked -= ShareClickedEventHandler;
            // _controlPanel.DeleteClicked -= DeleteClickedEventHandler;
            // _openPanelButton.onClick.RemoveListener(OpenPanelEventHandler);
            
            _lowerPanel.EditButtonEvent -= OpenPanelEventHandler;
            _lowerPanel.SaveButtonEvent -= SaveButtonEventHandler;
            _lowerPanel.DeleteButtonEvent -= DeleteClickedEventHandler;
            
            _credits.UpgradeBalanceEvent -= UpgradeBalanceEventHandler;
        }

        public void LoadPreviousImage(ImageData imageData)
        {
            _currentImage = imageData;
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(imageData.FileContent);
            texture.Apply();
            CurrentMaterial.mainTexture = texture;
        }

        public void HideControlPanel()
        {
            ControlPanelSetActive(false);
            _isPanelOpened = false;
        }
        
        private void SaveButtonEventHandler()
        {
            // save image
        }

        private void ControlPanelSetActive(bool isActive)
        {
            _controlPanel.SetActive(isActive);
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

            if (_currentTexture == null)
            {
                return;
            }

            byte[] bytes = _currentTexture.EncodeToJPG();
            _currentImage = new ImageData {FileContent = bytes, WallId = _wallId, Description = _controlPanel.InputField.text};
            _imageDataManager.SaveImage(_currentImage);
        }

        [ContextMenu("Save image")]
        private void ShareClickedEventHandler()
        {
            _imageDataManager.ShareImage(_currentImage);
            UploadImage();
        }

        private async void UploadImage()
        {
            await _backend.UploadImageData(_currentImage);
        }
        
        private void UpgradeBalanceEventHandler()
        {
            // sent email to admin with request possibility
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
            ControlPanelSetActive(_isPanelOpened);
        }

        [ContextMenu("Generate image")]
        private void GenerateImage()
        {
            GenerateImageEventHandler();
        }
    }
}