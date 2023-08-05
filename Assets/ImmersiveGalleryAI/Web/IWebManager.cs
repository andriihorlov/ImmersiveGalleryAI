using System.Threading.Tasks;
using UnityEngine;

namespace ImmersiveGalleryAI.Web
{
    public interface IWebManager
    {
        Task<Texture2D> GenerateImageEventHandler(string text);
        void Init(Sprite [] randomSprites, bool isAi);
    }
}