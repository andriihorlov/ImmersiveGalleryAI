using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace ImmersiveGalleryAI.Main.ImageData
{
    [Serializable]
    public class AllImages
    {
        public List<ImageData> ImagesData;
    }

    [Serializable]
    public class ImageData
    {
        public int WallId;
        public string FileName;
        public byte[] FileContent;
        public string FilePath;
        public string Description;
    }
}