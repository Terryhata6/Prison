using System;
using System.Collections;
using Game.Data;
using Game.Infrastructure;
using Game.Infrastructure.Services;
using UnityEngine;

namespace Game.UI.Interfaces
{
    public interface IUIService : IService
    {
        void EndCaveLevelCoroutine(ICoroutineRunner coroutineRunner, EndLevelData data, Action callback);
        void SetState(UIState state);
        void UpdateUIMoney(float money);
        void UpdateUiInventory(string value, Color textColor);
        void UpdateCopUi(float copTimer, float maximumCopTimer);
    }
}