using System;
using System.Threading.Tasks;
using OpenAI;
using UnityEngine;
using UnityEngine.Networking;

namespace ImmersiveGalleryAI.Common.Web
{
    [Serializable]
    public class OpenAiSettings
    {
        public string Api;
        public int ImageSize;
        public int Model;
    }
    
    public class WebManager : IWebManager
    {
        private const int DelayForRandomImage = 2000;
        private OpenAIApi _openAi = new OpenAIApi();

        private OpenAiSettings _openAiSettings;
        private Texture2D[] _randomSprites;
        private bool _isAi = false;
        
        private int _randomIndex = 0;

        public string ErrorMessage { get; private set; }

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

        public void Init(Texture2D[] randomSprites, bool isAi, OpenAiSettings settings)
        {
            _randomSprites = randomSprites;
            _isAi = isAi;

            _openAiSettings = settings;
            _openAi = new OpenAIApi(_openAiSettings.Api);
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
                Size = GetImageSize(),
                N = _openAiSettings.Model,
            });
            
            Texture2D createdTexture = null;

            if (response.Data?.Count > 0)
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
                ErrorMessage = response.Error.Message;
                Debug.LogWarning("No image was created from this prompt.");
            }
        
            return createdTexture;
        }

        private string GetImageSize() => _openAiSettings.ImageSize + "x" + _openAiSettings.ImageSize;

        private Texture2D CreateTexture(byte[] imageData)
        {
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageData);
            texture.Apply();
            return texture;
        }
    }
}