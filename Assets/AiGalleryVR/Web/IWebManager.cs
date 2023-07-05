using System.Threading.Tasks;
using UnityEngine;

namespace AiGalleryVR.Web
{
    public interface IWebManager
    {
        Task<Sprite> GenerateImageEventHandler(string text);
        void Init(Sprite [] randomSprites, bool isAi);
    }
}