using System;
using System.Collections.Generic;

namespace ImmersiveGalleryAI.Data
{
    [Serializable]
    public class ImageData
    {
        public int WallId;
        public string FileName;
        public byte[] FileContent;
        public string FilePath;
    }

    [Serializable]
    public class SettingsData
    {
        public List<ImageData> ImagesData;
    }
}