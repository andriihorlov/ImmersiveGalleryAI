using System;
using System.Collections.Generic;
using System.Linq;
using ImmersiveGalleryAI.Data;
using ImmersiveGalleryAI.ImageHandler;
using UnityEngine;
using Zenject;

namespace ImmersiveGalleryAI
{
    public class AppController : MonoBehaviour
    {
        [SerializeField] private List<WallImage> _images;
        
        [Inject] private IDataManager _dataManager;

        private void Awake()
        {
            _dataManager.LoadSettings();
        }

        private void Start()
        {
            LoadPreviousImages();
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

        private void OnApplicationQuit()
        {
            _dataManager.SaveSettings();
        }
    }
}
