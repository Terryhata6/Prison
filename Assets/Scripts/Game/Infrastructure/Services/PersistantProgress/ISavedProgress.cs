using Game.Data;

namespace Game.Infrastructure.Services.PersistantProgress
{
    public interface ISavedProgressReader
    {
        void LoadProgress(PlayerProgress progress);
    }

    public interface ISavedProgress : ISavedProgressReader
    {
        void UpdateProgress(PlayerProgress progress, string currentLevel = null);
    }
}