using System;
using System.Threading.Tasks;
using AiGalleryVR.Keyboard;
using AiGalleryVR.Utilities;
using AiGalleryVR.Web;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace AiGalleryVR.ImageHandler
{
    public class ImageGenerator : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _requestImageButton;
        [SerializeField] private Image _resultedImage;
        [SerializeField] private Loader.Loader _loadingLabel;

#region Physical buttons

        // [Header("Physical buttons")]
        // [SerializeField]
        // private TriggerEventReceiver _inputFieldEventReceiver;
        //
        // [SerializeField] private TriggerEventReceiver _generateButtonEventReceiver;
#endregion

        [Inject] private IWebManager _webManager;
        [Inject] private IKeyboard _keyboard;

        private void Awake()
        {
            _keyboard.Target = _inputField;
        }

        private void OnEnable()
        {
            _requestImageButton.onClick.AddListener(GenerateImageEventHandler);
            _inputField.onSelect.AddListener(InputFieldSelectedEventHandler);

#region Physical button

            // _inputFieldEventReceiver.TriggerEnter += InputFieldEventHandler;
            // _generateButtonEventReceiver.TriggerEnter += GenerateImageTriggerEventHandler;

#endregion
        }

        private void OnDisable()
        {
            _requestImageButton.onClick.RemoveListener(GenerateImageEventHandler);
            _inputField.onSelect.RemoveListener(InputFieldSelectedEventHandler);

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