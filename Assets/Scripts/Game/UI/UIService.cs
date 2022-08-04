using System;
using System.Collections;
using Game.Data;
using Game.Infrastructure;
using Game.Infrastructure.Factory;
using Game.UI.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIService : IUIService
    {
        private IGameFactory _gameFactory;
        private UIController _uiController;
        
        

        public UIService(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            _uiController = _gameFactory.CreateUI().GetComponent<UIController>();
            _uiController.Init();
        }

        public void EndCaveLevelCoroutine(ICoroutineRunner coroutineRunner, EndLevelData data, Action callback)
        {
            SetState(UIState.EndGame);
            coroutineRunner.StartCoroutine(_uiController.EndCaveLevelCoroutine(data, callback));
        }

        public void SetState(UIState state)
        {
            _uiController.SetState(state);
        }

        public void UpdateUIMoney(float money)
        {
            _uiController.UpdateMoney(money);
        }

        public void UpdateUiInventory(string value, Color textColor)
        {
            _uiController.UpdateInventory(value, textColor);
            
        }

        public void UpdateCopUi(float copTimer, float maximumCopTimer)
        {
            _uiController.UpdateCopUI(copTimer, maximumCopTimer);
        }
    }
}