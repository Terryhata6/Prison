using System.Collections.Generic;
using Game.UI.Interfaces;
using Game.UI.Panels;
using UnityEngine;

namespace Game.UI
{
    public class UIController : MonoBehaviour
    {
        public UIState State;
        public Dictionary<UIState, UIPanel> _panels = new Dictionary<UIState, UIPanel>();
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _panels.Add(UIState.Ingame, GetComponentInChildren<InGameUI>());
            _panels.Add(UIState.EndGame, GetComponentInChildren<EndGameUI>());
            _panels.Add(UIState.Pause, GetComponentInChildren<PauseUI>());
            
            SetState(UIState.Bootstrap);
        }

        public void Init()
        {
            
        }

        public void SetState(UIState state)
        {
            if(State == state)
                return;
            foreach (var panel in _panels) panel.Value.Hide();
            if(_panels.ContainsKey(state))
                _panels[state].Show();
            State = state;
        }
    }
}