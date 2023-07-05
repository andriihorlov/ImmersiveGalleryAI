using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using VRUiKits.Utils;

namespace UIKits.Scripts.Editor
{
    public class KeyboardSettingsEditor : EditorWindow
    {
        private bool _isKeyboardPhysical;

        [MenuItem("Window/Keyboard/Keyboard settings", false, 10000)]
        public static void ShowKeyboardSettings()
        {
            KeyboardSettingsEditor wnd = GetWindow<KeyboardSettingsEditor>();
            wnd.titleContent = new GUIContent("Keyboard settings");
            wnd.Show();
        }

        public void OnGUI()
        {
            GUILayout.Label("Keyboard settings");
            _isKeyboardPhysical = GUILayout.Toggle(_isKeyboardPhysical, "Is keyboard physical");
            if (GUILayout.Button("Apply"))
            {
                ApplyButtonClicked();
            }
        }

        private void ApplyButtonClicked()
        {
            Key[] keys = FindObjectsOfType<Key>();
            foreach (Key key in keys)
            {
                if (_isKeyboardPhysical)
                {
                    Collider collider = GetOrAddComponent<Collider>(key);
                    Rigidbody rigidbody = GetOrAddComponent<Rigidbody>(key);
                    DestroyComponentIfExist<Button>(key);

                    rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                    rigidbody.isKinematic = true;
                    collider.isTrigger = true;
                }
                else
                {
                    Button button = GetOrAddComponent<Button>(key);
                    if (!button.enabled)
                    {
                        button.enabled = true;
                    }
                    DestroyComponentIfExist<Rigidbody>(key);
                    DestroyComponentIfExist<Collider>(key);
                }
            }
        }

        private T GetOrAddComponent<T>(Key key) where T : Component
        {
            T component = key.gameObject.GetComponent<T>();
            if (component == null)
            {
                component = key.gameObject.AddComponent<T>();
            }

            return component;
        }

        private void DestroyComponentIfExist<T>(Key key) where T : Component
        {
            T component = key.gameObject.GetComponent<T>();
            if (component != null)
            {
                DestroyImmediate(component);
            }
        }
    }
}