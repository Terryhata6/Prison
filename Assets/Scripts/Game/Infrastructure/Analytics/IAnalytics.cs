using Game.Infrastructure.Services;

namespace Game.Infrastructure.Analytics
{
    public interface IAnalytics : IService
    {
        void OnLevelStart();
        void OnLevelFailed();
        void OnLevelVictory();
    }
}