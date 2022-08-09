using Game.Infrastructure.Services;

namespace Game.Infrastructure.States
{
    public interface ISoundController : IService
    {
        void PlaySound(string id);
    }
}