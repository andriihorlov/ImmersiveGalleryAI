using TMPro;
using UnityEngine;
using UnityEngine.Events;
using VRUiKits.Utils;

namespace ImmersiveGalleryAI.Common.Keyboard
{
    public interface IKeyboard 
    {
        event UnityAction EnterPressedAction;
        event UnityAction ClosePressedAction;
        event UnityAction<string> InputAction;

        TMP_InputField Target { set; }
        bool IsActive { get; }
        
        void Init(KeyboardData keyboardData);
        void SetActive(bool isActive, bool isImmediate = false);
        void ChangePosition(Transform pivot);
    }
}
