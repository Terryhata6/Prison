using Game.Data;

namespace Game.Infrastructure.Services.PersistantProgress
{
    public class PersistantProgressService : IPersistantProgressService
    {
        public PlayerProgress Progress { get; set; }
    }
}