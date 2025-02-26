﻿using System.Threading.Tasks;
using UnityEngine;

namespace ImmersiveGalleryAI.Common.Web
{
    public interface IWebManager
    {
        string ErrorMessage { get; }
        Task<Texture2D> GenerateImageEventHandler(string text);
        void Init(Texture2D [] randomSprites, bool isAi, string api);
    }
}