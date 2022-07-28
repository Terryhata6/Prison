using Game.Infrastructure.AssetManagment;
using Game.Infrastructure.Services;

namespace Game.Infrastructure.Factory
{
    public class LevelCreatorService : IService
    {
        private readonly IAssets assetProvider;

        public LevelCreatorService(IAssets assetProvider)
        {
            this.assetProvider = assetProvider;
        }
        
        
        
    }
}