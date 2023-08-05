using UnityEngine;

namespace ImmersiveGalleryAI.User
{
    public interface IUser 
    {
        Transform CameraRigTransform { get; }
        void Init(UserData userData);
    }
}
