using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ImmersiveGalleryAI.Common.SceneManagement
{
    public class SceneManager : ISceneManager
    {
        private Dictionary<SceneType, string> SceneName = new Dictionary<SceneType, string>()
        {
            {SceneType.Lobby, "Lobby"},
            {SceneType.Main, "Main"}
        };

        private Scene _currentScene;

        public async void LoadScene(SceneType sceneType)
        {
            string targetScene = SceneName[sceneType];

            if (!string.IsNullOrEmpty(_currentScene.name))
            {
                AsyncOperation unloadingScene = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(_currentScene.name);
                await unloadingScene;
            }

            _currentScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(targetScene);

            AsyncOperation loadingScene = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_currentScene.name, LoadSceneMode.Additive);
            loadingScene.allowSceneActivation = false;
            // while (loadingScene.progress < 0.9f)
            // {
            //     await UniTask.Yield();
            // }

            await loadingScene;

            loadingScene.allowSceneActivation = true;
        }
    }
}