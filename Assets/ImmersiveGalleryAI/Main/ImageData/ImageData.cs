using System;
using System.Collections.Generic;

namespace ImmersiveGalleryAI.Main.ImageData
{
    [Serializable]
    public class SettingsData
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