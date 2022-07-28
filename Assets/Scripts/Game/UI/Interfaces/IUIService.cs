using System;
using System.Collections;
using Game.Data;
using Game.Infrastructure.Services;

namespace Game.UI.Interfaces
{
    public interface IUIService : IService
    {
        IEnumerator EndLevelCoroutine(EndLevelData data, Action callback);
        void SetState(UIState state);
    }
}