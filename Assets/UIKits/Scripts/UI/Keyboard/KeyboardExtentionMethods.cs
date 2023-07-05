/***
 * Author: Yunhan Li
 * Any issue please contact yunhn.lee@gmail.com
 ***/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace VRUiKits.Utils
{
    public class KeyboardExtentionMethods : MonoBehaviour
    {
        [SerializeField] private List<Keyboard> _keyboards;
        private bool _isSymbolsOpened;

        private void Awake()
        {
            if (_keyboards == null)
            {
                Debug.LogError("Please assign Keyboards game objects");
                return;
            }

            SwitchKeyboard(KeyboardType.Alphabets);
        }

        public void SwitchToAbc()
        {
            SwitchKeyboard(KeyboardType.Alphabets);
        }

        public void SwitchToSymbols()
        {
            _isSymbolsOpened = !_isSymbolsOpened;
            foreach (Keyboard keyboard in _keyboards)
            {
                if (keyboard.KeyboardType == KeyboardType.Symbols)
                {
                    keyboard.KeyboardGameObject.SetActive(_isSymbolsOpened);
                }
            }
        }

        public void SwitchKeyboard(KeyboardType targetKeyboard)
        {
            foreach (var keyboard in _keyboards)
            {
                keyboard.KeyboardGameObject.SetActive(keyboard.KeyboardType.Equals(targetKeyboard));
            }
        }
    }

    [Serializable]
    public struct Keyboard
    {
        public KeyboardType KeyboardType;
        public GameObject KeyboardGameObject;
    }

    public enum KeyboardType
    {
        Alphabets,
        Symbols,
        NumsWithoutAlphabets
    }
}