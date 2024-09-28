using System.Collections.Generic;
using System.Linq;
using ImmersiveGalleryAI.Common.Keyboard;
using ImmersiveGalleryAI.Common.PlayerLocation;
using ImmersiveGalleryAI.Common.VoiceRecognition;
using ImmersiveGalleryAI.Main.ImageData;
using ImmersiveGalleryAI.Main.ImageHandler;
using UnityEngine;
using Zenject;

namespace ImmersiveGalleryAI.Main
{
    public class ImagesController : MonoBehaviour
    {
        [SerializeField] private List<WallImage> _images;

        private WallImage _currentImagePanel;
        private bool _isMicEnabled;

        [Inject] private IVoiceHandler _voiceHandler;
        [Inject] private IImageDataManager _imageDataManager;
        [Inject] private IKeyboard _keyboard;
        [Inject] private IPlayerLocation _playerLocation;

        private void Awake()
        {
            _imageDataManager.LoadSettings();
        }

        private void OnEnable()
        {
            _voiceHandler.TranscriptionDoneEvent += RequestTranscriptEventHandler;
            _voiceHandler.StoppedListeningEvent += StoppedListeningEventHandler;

            foreach (WallImage wallImage in _images)
            {
                wallImage.OpenedPanel += OpenedPanelEventHandler;
            }
        }

        private void Start()
        {
            LoadPreviousImages();
        }

        private void OnDisable()
        {
            _voiceHandler.TranscriptionDoneEvent -= RequestTranscriptEventHandler;
            _voiceHandler.StoppedListeningEvent -= StoppedListeningEventHandler;

            foreach (WallImage wallImage in _images)
            {
                wallImage.OpenedPanel -= OpenedPanelEventHandler;
                wallImage.ControlPanel.InputFieldSelected -= InputFieldSelectedEventHandler;
                wallImage.ControlPanel.VoiceClicked -= VoiceClickedEventHandler;
            }
        }

        private void OnApplicationQuit()
        {
            _imageDataManager.SaveSettings();
        }

        public void SetActive(bool isActive)
        {
            foreach (WallImage wallImage in _images)
            {
                wallImage.SetActive(isActive);
            }
        }

        private void LoadPreviousImages()
        {
            if (_imageDataManager.Settings?.ImagesData == null)
            {
                return;
            }

            foreach (ImageData.ImageData imageData in _imageDataManager.Settings.ImagesData)
            {
                WallImage targetWall = _images.FirstOrDefault(t => t.WallId.Equals(imageData.WallId));
                if (targetWall != null)
                {
                    targetWall.LoadPreviousImage(imageData);
                }
            }
        }

        private void OpenedPanelEventHandler(WallImage wallImage)
        {
            if (_currentImagePanel != null)
            {
                if (_currentImagePanel != wallImage)
                {
                    _currentImagePanel.HideControlPanel();
                }

                _currentImagePanel.ControlPanel.InputFieldSelected -= InputFieldSelectedEventHandler;
                _currentImagePanel.ControlPanel.VoiceClicked -= VoiceClickedEventHandler;
            }

            _currentImagePanel = wallImage;
            wallImage.ControlPanel.InputFieldSelected += InputFieldSelectedEventHandler;
            wallImage.ControlPanel.VoiceClicked += VoiceClickedEventHandler;

            _keyboard.Target = wallImage.ControlPanel.InputField;
            if (_keyboard.IsActive)
            {
                _keyboard.ChangePosition(_playerLocation.CameraRigTransform);
            }

            _keyboard.SetActive(true);
        }

        private void InputFieldSelectedEventHandler()
        {
            if (_keyboard.IsActive)
            {
                return;
            }

            _keyboard.ChangePosition(_playerLocation.CameraRigTransform);
            _keyboard.SetActive(true);
        }


#region Voice recognition

        private void StoppedListeningEventHandler()
        {
            ToggleMicButton(false);
        }

        private void RequestTranscriptEventHandler(string transcript)
        {
            _currentImagePanel.ControlPanel.InputField.text = transcript;
            ToggleMicButton(false);
        }

        private void ToggleMicButton(bool isActive)
        {
            _isMicEnabled = isActive;
            _currentImagePanel.ControlPanel.ToggleMicText(isActive);
        }

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

#endregion

#if UNITY_EDITOR
        [ContextMenu("Fetch all Wall Images")]
        private void FetchWallImagesEditor()
        {
            _images = FindObjectsOfType<WallImage>().ToList();
        }
#endif
    }
}