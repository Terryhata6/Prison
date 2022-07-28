using System.Collections;

namespace Game.Infrastructure.Services.UI
{
    public interface IUIService : IService
    {
        public IEnumerator EndLevelCoroutine();
    }
}