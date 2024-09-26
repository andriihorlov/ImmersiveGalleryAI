using UnityEngine;
using UnityEngine.SceneManagement;

namespace ImmersiveGalleryAI.ProjectContext.Scene
{
    public class ProjectContextRunScene : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.LoadSceneAsync("Lobby", LoadSceneMode.Additive);
        }
    }
}
