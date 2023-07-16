/***
 * Author: Yunhan Li
 * Any issue please contact yunhn.lee@gmail.com
 ***/

using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using ImmersiveGalleryAI.Keyboard;
using TMPro;
using UnityEngine.Events;

namespace VRUiKits.Utils
{
    public class KeyboardManager : IKeyboard
    {
        private const float FadeDuration = 0.3f;

        public event UnityAction EnterPressedAction;
        public event UnityAction ClosePressedAction;
        public event UnityAction<string> InputAction;

        private static UnityAction<Transform> ChangePositionAction;

        private KeyboardData _keyboardData;
        private Transform _keyboardTransform;

#region Public Variables

        public TMP_InputField Target
        {
            get
            {
                if (_targetInputField != null)
                {
                    return _targetInputField;
                }

#if UNITY_EDITOR
                GameObject selectedGameObject = EventSystem.current.currentSelectedGameObject;
                if (selectedGameObject != null && selectedGameObject.GetComponent<TMP_InputField>() != null)
                {
                    return selectedGameObject.GetComponent<TMP_InputField>();
                }
#endif

                return _targetInputField != null ? _targetInputField : null;
            }
            set => _targetInputField = value;
        }

        public bool IsActive { get; private set; }

#endregion

#region Private Variables

        /*
         Record a helper target for some 3rd party packages which lost focus when
         user click on keyboard.
         */
        private static TMP_InputField _targetInputField;

        private string Input
        {
            get => null == Target ? "" : Target.text;
            set
            {
                if (null == Target)
                {
                    return;
                }

                Target.text = value;
                // Force target input field activated if losing selection
                Target.ActivateInputField();
                Target.MoveTextEnd(false);
            }
        }

        private bool _capslockFlag;
        private Transform _initialParent;

#endregion

#region Monobehaviour Callbacks

#endregion

#region Public Methods

        public void Init(KeyboardData keyboardData)
        {
            _keyboardData = keyboardData;
            InitOnStart();
            _keyboardTransform = keyboardData.KeyboardTransform;
        }

        public void ChangePosition(Transform pivot)
        {
            _keyboardTransform.SetPositionAndRotation(pivot.position, pivot.rotation);
            _keyboardTransform.SetParent(pivot.parent);
            _keyboardTransform.localScale = pivot.localScale;
        }

        public void SetActive(bool isActive, bool isImmediate = false)
        {
            float targetAlpha = isActive ? 1 : 0;
            _keyboardData.CanvasGroup.interactable = isActive;
            _keyboardData.CanvasGroup.blocksRaycasts = isActive;
            IsActive = isActive;

            if (isImmediate)
            {
                _keyboardData.CanvasGroup.alpha = targetAlpha;
                return;
            }
            
            _keyboardData.CanvasGroup.DOFade(targetAlpha, FadeDuration);
        }

#endregion

        private void InitOnStart()
        {
            foreach (Key key in _keyboardData.KeyList)
            {
                key.OnKeyClicked += GenerateInput;

// #if !UNITY_EDITOR
//                 key.IsPhysicalButton = _keyboardData.IsPhysicalButton;
// #else
//                 key.IsPhysicalButton = false;
// #endif
            }

            CapsLock();
            SetActive(false, isImmediate: true);

            _keyboardData.BackspaceButton.onClick.AddListener(Backspace);
            _keyboardData.ClearButton.onClick.AddListener(ClearAll);
            _keyboardData.EnterButton.onClick.AddListener(Enter);
            _keyboardData.CapslockButton.onClick.AddListener(CapsLock);
        }

#region ButtonHandlers

        private void Backspace()
        {
            if (Input.Length > 0)
            {
                Input = Input.Remove(Input.Length - 1);
            }
        }

        private void ClearAll()
        {
            Input = "";
        }

        private void CapsLock()
        {
            foreach (Key key in _keyboardData.KeyList)
            {
                if (key is Alphabet)
                {
                    key.CapsLock(_keyboardData.IsUpperCase);
                }
            }

            _keyboardData.IsUpperCase = !_keyboardData.IsUpperCase;
        }

        private void Shift()
        {
            foreach (Key key in _keyboardData.KeyList)
            {
                if (key is Shift)
                {
                    key.ShiftKey();
                }
            }
        }

        private void Enter()
        {
            EnterPressedAction?.Invoke();
            SetActive(false);
        }

        private void Close()
        {
            ClosePressedAction?.Invoke();
            SetActive(false);
        }
        
        private void GenerateInput(string newSymbol)
        {
            Input += newSymbol;
            InputAction?.Invoke(newSymbol);
        }

#endregion

        ~KeyboardManager()
        {
            foreach (Key key in _keyboardData.KeyList)
            {
                key.OnKeyClicked -= GenerateInput;
            }

            _keyboardData.BackspaceButton.onClick.RemoveListener(Backspace);
            _keyboardData.ClearButton.onClick.RemoveListener(ClearAll);
            _keyboardData.EnterButton.onClick.RemoveListener(Enter);
            _keyboardData.CapslockButton.onClick.RemoveListener(CapsLock);
        }
    }
}