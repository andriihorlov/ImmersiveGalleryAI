using System;
using System.Threading.Tasks;
using ImmersiveGalleryAI.Data;
using ImmersiveGalleryAI.Loader;
using ImmersiveGalleryAI.Web;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ImmersiveGalleryAI.ImageHandler
{
    public class WallImage : MonoBehaviour
    {
        public event Action<WallImage> OpenedPanel;

        [SerializeField] private int _wallId;
        [SerializeField] private Image _resultedImage;
        [SerializeField] private LoaderHandler _loadingLabel;
        [SerializeField] private ControlPanel _controlPanel;
        [SerializeField] private Button _openPanelButton;

        private ImageData _currentImage;
        private Texture2D _currentTexture;
        private bool _isPanelOpened;

        [Inject] private IWebManager _webManager;
        [Inject] private IDataManager _dataManager;

        public int WallId => _wallId;
        public ControlPanel ControlPanel => _controlPanel;

        private void OnEnable()
        {
            _controlPanel.GenerateImageClicked += GenerateImageEventHandler;

            _controlPanel.ShareClicked += ShareClickedEventHandler;
            _controlPanel.DeleteClicked += DeleteClickedEventHandler;
            _openPanelButton.onClick.AddListener(OpenPanelEventHandler);
        }

        private void OnDisable()
        {
            _controlPanel.GenerateImageClicked -= GenerateImageEventHandler;
            _controlPanel.ShareClicked -= ShareClickedEventHandler;
            _controlPanel.DeleteClicked -= DeleteClickedEventHandler;
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

        public void HideControlPanel()
        {
            ControlPanelSetActive(false);
            _isPanelOpened = false;
        }

        private void ControlPanelSetActive(bool isActive)
        {
            _controlPanel.SetActive(isActive);
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

            if (_currentTexture == null)
            {
                return;
            }

            _currentImage = new ImageData {FileContent = _currentTexture.EncodeToJPG(), WallId = _wallId, Description = _controlPanel.InputField.text};
            _dataManager.SaveImage(_currentImage);
        }

        private Sprite CreateSprite(Texture2D texture2D)
        {
            return Sprite.Create(texture2D, new Rect(0, 0, 256, 256), Vector2.zero);
        }

        private void ShareClickedEventHandler()
        {
            _dataManager.ShareImage(_currentImage);
        }

        [ContextMenu("Delete currentImage")]
        private void DeleteClickedEventHandler()
        {
            _dataManager.DeleteImage(_currentImage);
            _currentImage = null;
            _currentTexture = null;
            _resultedImage.sprite = null;
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