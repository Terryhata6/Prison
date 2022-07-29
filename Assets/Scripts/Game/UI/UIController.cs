using System;
using System.Collections;
using System.Collections.Generic;
using Game.Data;
using Game.Infrastructure.Services;
using Game.Infrastructure.Services.SaveLoad;
using Game.UI.Interfaces;
using Game.UI.Panels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIController : MonoBehaviour
    {
        public UIState State;
        public Dictionary<UIState, UIPanel> _panels = new Dictionary<UIState, UIPanel>();
        
        public Button _uiNextLevelButton;
        public GameObject SuccsessEscapeUI;
        public GameObject CatchedUI;
        public TMP_Text EarnedCashUI;
        public TMP_Text CopCashUI;

        public TMP_Text InGameMoney;
        
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

        public IEnumerator EndCaveLevelCoroutine(EndLevelData data, Action callback)
        {
            
            if (data.EscapeResult)
            {
                SuccsessEscapeUI.SetActive(true);
                CatchedUI.SetActive(false);
            }
            else
            {
                SuccsessEscapeUI.SetActive(false);
                CatchedUI.SetActive(true);
            }
            
            EarnedCashUI.text = "You get: " + data.EarnedCash.ToString()+"$";
            CopCashUI.text = "Cop get: " + data.CopCash.ToString()+"$";
            
            bool flag = false;
            _uiNextLevelButton.onClick.AddListener(() =>
            {
                Debug.Log("ButtonDown");
                flag = true;
                callback?.Invoke();
                _uiNextLevelButton.onClick.RemoveAllListeners();
            });
            
            yield return new WaitUntil(() => flag);
            
            
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

        public void UpdateMoney(float money)
        {
            InGameMoney.text = money.ToString()+"$";
        }
    }
}