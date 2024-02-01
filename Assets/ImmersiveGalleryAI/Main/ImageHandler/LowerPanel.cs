using System;
using UnityEngine;
using UnityEngine.UI;

namespace ImmersiveGalleryAI.Main.ImageHandler
{
    public class LowerPanel : MonoBehaviour
    {
        [SerializeField] private Button _editButton;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _deleteButton;

        public event Action EditButtonEvent;
        public event Action SaveButtonEvent;
        public event Action DeleteButtonEvent;

        private void OnEnable()
        {
            _editButton.onClick.AddListener(EditButtonClicked);
            _saveButton.onClick.AddListener(SaveButtonClicked);
            _deleteButton.onClick.AddListener(DeleteButtonClicked);
        }

        private void OnDisable()
        {
            _editButton.onClick.RemoveListener(EditButtonClicked);
            _saveButton.onClick.RemoveListener(SaveButtonClicked);
            _deleteButton.onClick.RemoveListener(DeleteButtonClicked);
        }


        private void EditButtonClicked()
        {
            EditButtonEvent?.Invoke();
        }

        private void SaveButtonClicked()
        {
            SaveButtonEvent?.Invoke();
        }

        private void DeleteButtonClicked()
        {
            DeleteButtonEvent?.Invoke();
        }
    }
}
