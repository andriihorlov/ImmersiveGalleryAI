using System;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace VRUiKits.Utils
{
    public class Alphabet : Key
    {
        [SerializeField] private string _newKey;

        public void SetKey()
        {
            if (key == null)
            {
                key = GetComponentInChildren<TextMeshProUGUI>();
            }
            if (string.IsNullOrEmpty(_newKey))
            {
                Debug.LogWarning($"Please fill the key for the -> {gameObject.name}");
                return;
            }
            key.text = _newKey;
            gameObject.name = _newKey;
        }

        public override void CapsLock(bool isUppercase)
        {
            if (key == null)
            {
                return;
            }

            if (isUppercase)
            {
                key.text = key.text.ToUpper();
            }
            else
            {
                try
                {
                    key.text = key.text.ToLower();
                }
                catch (Exception e)
                {
                    Debug.Log($"{e.Message}\n{transform.parent.parent.name}/{transform.parent.name}/{transform.name}");
                }

            }
        }
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(Alphabet))]
    public class AlphabetEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Set key"))
            {
                ((Alphabet) target).SetKey();
            }
        }
    }
    #endif
}