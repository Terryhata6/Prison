using Game.Data;
using Unity.VisualScripting;

namespace Game.Infrastructure.Services.PersistantProgress
{
    public interface IPersistantProgressService : IService
    {
        PlayerProgress Progress { get; set; }
    }
}