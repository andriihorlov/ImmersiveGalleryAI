using System.Threading.Tasks;
using OpenAI;
using UnityEngine;
using UnityEngine.Networking;

namespace ImmersiveGalleryAI.Common.Web
{
    public class WebManager : IWebManager
    {
        private const int DelayForRandomImage = 2000;
        private OpenAIApi _openAi = new OpenAIApi();
        
        private Texture2D[] _randomSprites;
        private bool _isAi = false;
        
        private int _randomIndex = 0;

        public async Task<Texture2D> GenerateImageEventHandler(string text)
        {
            Texture2D resultedSprite = null;
        
            if (_isAi)
            {
                Task<Texture2D> resulted = GenerateImageAi(text);
                await resulted;
                resultedSprite = resulted.Result;
            }
            else
            {
                await Task.Delay(DelayForRandomImage);
                resultedSprite = GetNextImage();
                _randomIndex = GetNextIndex();
            }
        
            return resultedSprite;
        }

        public void Init(Texture2D[] randomSprites, bool isAi, string api)
        {
            _randomSprites = randomSprites;
            _isAi = isAi;
            _openAi = new OpenAIApi(api);
        }

        private Texture2D GetNextImage() => _randomSprites[_randomIndex];
        
        private int GetNextIndex()
        {
            _randomIndex++;
            if (_randomIndex > _randomSprites.Length - 1)
            {
                _randomIndex = 0;
            }
        
            return _randomIndex;
        }
        
        //private int GetSpriteIndex() => Random.Range(0, _randomSprites.Length - 1);
        
        private async Task<Texture2D> GenerateImageAi(string text)
        {
            CreateImageResponse response = await _openAi.CreateImage(new CreateImageRequest
            {
                Prompt = text,
                Size = ImageSize.Size256
            });
        
            Texture2D createdTexture = null;
        
            if (response.Data.Count > 0)
            {
                using UnityWebRequest request = new UnityWebRequest(response.Data[0].Url)
                {
                    downloadHandler = new DownloadHandlerBuffer()
                };
        
                request.SetRequestHeader("Access-Control-Allow-Origin", "*");
                request.SendWebRequest();

                while (!request.isDone)
                {
                    await Task.Yield();
                }
                createdTexture = CreateTexture(request.downloadHandler.data);
            }
            else
            {
                Debug.LogWarning("No image was created from this prompt.");
            }
        
            return createdTexture;
        }
        
        private Texture2D CreateTexture(byte[] imageData)
        {
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageData);
            texture.Apply();
            return texture;
        }
    }
}