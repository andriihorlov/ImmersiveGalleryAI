using System.Collections.Generic;
using System.Linq;
using ImmersiveGalleryAI.Data;
using ImmersiveGalleryAI.ImageHandler;
using ImmersiveGalleryAI.Keyboard;
using ImmersiveGalleryAI.User;
using ImmersiveGalleryAI.VoiceRecognition;
using UnityEngine;
using Zenject;

namespace ImmersiveGalleryAI
{
    public class AppController : MonoBehaviour
    {
        [SerializeField] private List<WallImage> _images;

        private WallImage _currentImagePanel;
        private bool _isMicEnabled;

        [Inject] private IVoiceHandler _voiceHandler;
        [Inject] private IDataManager _dataManager;
        [Inject] private IKeyboard _keyboard;
        [Inject] private IUser _user;

        private void Awake()
        {
            _dataManager.LoadSettings();
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
            _dataManager.SaveSettings();
        }

        private void LoadPreviousImages()
        {
            if (_dataManager.Settings?.ImagesData == null)
            {
                return;
            }

            foreach (ImageData imageData in _dataManager.Settings.ImagesData)
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
                _keyboard.ChangePosition(_user.CameraRigTransform);
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