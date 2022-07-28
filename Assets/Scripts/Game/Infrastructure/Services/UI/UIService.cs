using System.Collections;
using Game.Infrastructure.Factory;

namespace Game.Infrastructure.Services.UI
{
    public class UIService : IUIService
    {
        private IGameFactory _gameFactory;

        public UIService(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public IEnumerator EndLevelCoroutine()
        {
            yield return null;
        }
    }
}