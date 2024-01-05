using System.Threading.Tasks;
using UnityEngine;

namespace ImmersiveGalleryAI.Common.Web
{
    public interface IWebManager
    {
        Task<Texture2D> GenerateImageEventHandler(string text);
        void Init(Sprite [] randomSprites, bool isAi);
    }
}