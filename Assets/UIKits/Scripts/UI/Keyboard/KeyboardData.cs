using AiGalleryVR.Keyboard;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace VRUiKits.Utils
{
    public class KeyboardData : MonoBehaviour
    {
        [Header("User defined")]
        [Tooltip("If the character is uppercase at the initialization")]
        [SerializeField] private bool _isUppercase = false;
        [field: SerializeField] public bool IsPhysicalButton { get; private set; }

        [Header("Essentials")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Key[] _keyList;

        [field: SerializeField, Header("Buttons")] public Button EnterButton { get; private set; }
        [field: SerializeField] public Button ClearButton { get; private set; }
        [field: SerializeField] public Button CapslockButton { get; private set; }
        [field: SerializeField] public Button BackspaceButton { get; private set; }

        [Inject] private IKeyboard _keyboard;
        public bool IsUpperCase
        {
            get => _isUppercase;
            set => _isUppercase = value;
        }

        public CanvasGroup CanvasGroup => _canvasGroup;
        public Key[] KeyList => _keyList;
        public Transform KeyboardTransform => transform;

        private void Awake()
        {
            if (_keyList?.Length < 1)
            {
                Debug.Log($"Please fetch all items via ContextMenu");
            }

            _keyboard.Init(this);
        }
        
        [ContextMenu("Fetch all keys")]
        private void FetchAllKeys()
        {
            _keyList = transform.GetComponentsInChildren<Key>(true); 
        }
    }
}