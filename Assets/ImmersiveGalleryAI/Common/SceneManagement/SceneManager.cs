using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ImmersiveGalleryAI.Common.SceneManagement
{
    public class SceneManager : ISceneManager
    {
        private Dictionary<SceneType, string> SceneName = new Dictionary<SceneType, string>()
        {
            {SceneType.Lobby, "Lobby"},
            {SceneType.Main, "Main"}
        };

        public async void LoadScene(SceneType sceneType)
        {
            string targetScene = SceneName[sceneType];
            AsyncOperation loadingScene = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(targetScene);
            loadingScene.allowSceneActivation = false;
            while (loadingScene.progress < 0.9f)
            {
                await UniTask.Yield();
            }

            loadingScene.allowSceneActivation = true;
        }
    }
}