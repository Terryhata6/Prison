using System;
using System.Collections;
using Game.Data;
using Game.Infrastructure.Factory;
using Game.UI.Interfaces;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIService : IUIService
    {
        private IGameFactory _gameFactory;
        private Button _uiNextLevelButton;
        private UIController _uiController;

        public UIService(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            _uiController = _gameFactory.CreateUI().GetComponent<UIController>();
            _uiController.Init();
        }

        public IEnumerator EndLevelCoroutine(EndLevelData data, Action callback)
        {
            
            _uiNextLevelButton.onClick.AddListener(() =>
            {
                _uiNextLevelButton.onClick.RemoveAllListeners();    
            });
            
            
            SetState(UIState.EndGame);
            yield return null;
        }

        public void SetState(UIState state)
        {
            _uiController.SetState(state);
        }
    }
}