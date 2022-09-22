using Game.Infrastructure.Services;

namespace Game.Infrastructure.Music
{
    public interface IHighGroundVisualController : IService
    {
        void ChangeMaterialToNextInList();
    }
}