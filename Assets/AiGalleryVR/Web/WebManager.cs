using System.Threading.Tasks;
using OpenAI;
using UnityEngine;
using UnityEngine.Networking;

namespace AiGalleryVR.Web
{
    public class WebManager : IWebManager
    {
        private const int DelayForRandomImage = 2000;
        private readonly OpenAIApi _openAi = new OpenAIApi("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");
        
        private Sprite[] _randomSprites;
        private bool _isAi = false;
        
        private int _randomIndex = 0;

        public async Task<Sprite> GenerateImageEventHandler(string text)
        {
            Sprite resultedSprite = null;
        
            if (_isAi)
            {
                Task<Sprite> resulted = GenerateImageAi(text);
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

        public void Init(Sprite[] randomSprites, bool isAi)
        {
            _randomSprites = randomSprites;
            _isAi = isAi;
        }

        private Sprite GetNextImage() => _randomSprites[_randomIndex];
        
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
        
        private async Task<Sprite> GenerateImageAi(string text)
        {
            CreateImageResponse response = await _openAi.CreateImage(new CreateImageRequest
            {
                Prompt = text,
                Size = ImageSize.Size256
            });
        
            Sprite createdSprite = null;
        
            if (response.Data is {Count: > 0})
            {
                using UnityWebRequest request = new UnityWebRequest(response.Data[0].Url)
                {
                    downloadHandler = new DownloadHandlerBuffer()
                };
        
                request.SetRequestHeader("Access-Control-Allow-Origin", "*");
                request.SendWebRequest();
        
                while (!request.isDone) await Task.Yield();
                createdSprite = CreateSprite(request.downloadHandler.data);
            }
            else
            {
                Debug.LogWarning("No image was created from this prompt.");
            }
        
            return createdSprite;
        }
        
        private Sprite CreateSprite(byte[] imageData)
        {
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageData);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 256, 256), Vector2.zero, 1f);
            return sprite;
        }
    }
}