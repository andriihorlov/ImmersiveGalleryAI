namespace ImmersiveGalleryAI.Common.Backend
{
    public interface IBackend
    {
        void Registration(string email, string password);
        void Login(string login, string password);
    }
}