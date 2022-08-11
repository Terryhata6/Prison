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
        public TMP_Text InGameInventory;

        [Header("CopArrow")] public Transform _copHolder;
        public RectTransform _copArrow;
        public RectTransform _copArrowStart;
        public RectTransform _copArrowFinish;

        [Header("UserMessage")] public TMP_Text _userMessageText;

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

            EarnedCashUI.text =  data.EarnedCash + "$";
            CopCashUI.text =  data.CopCash + "$";

            bool flag = false;
            _uiNextLevelButton.onClick.AddListener(() =>
            {
                flag = true;
                callback?.Invoke();
                _uiNextLevelButton.onClick.RemoveAllListeners();
            });

            yield return new WaitUntil(() => flag);
        }

        public void SetState(UIState state)
        {
            if (State == state)
                return;
            foreach (var panel in _panels) panel.Value.Hide();
            if (_panels.ContainsKey(state))
                _panels[state].Show();
            State = state;
        }

        public void UpdateMoney(float money)
        {
            InGameMoney.text = money.ToString();
        }

        public void UpdateInventory(string value, Color textColor)
        {
            InGameInventory.text = value;
            InGameInventory.color = textColor;
        }

        public void UpdateCopUI(float copTimer, float maximumCopTimer)
        {
            if (copTimer <= 0)
            {
                _copArrow.position = Vector3.Lerp(_copArrowStart.position, _copArrowFinish.position,
                    copTimer / maximumCopTimer);
                _copHolder.gameObject.SetActive(false);
            }
            else
            {
                _copHolder.gameObject.SetActive(true);
                _copArrow.position= Vector3.Lerp(_copArrowStart.position, _copArrowFinish.position,
                    copTimer / maximumCopTimer);
            }
        }

        public void CallUserMessage(string message, float duration)
        {
            StartCoroutine(CallUserMessageCoroutine(message, duration));
        }

        private IEnumerator CallUserMessageCoroutine(string message, float duration)
        {
            _userMessageText.text = message;
            _userMessageText.gameObject.SetActive(true);
            yield return new WaitForSeconds(duration);
            _userMessageText.gameObject.SetActive(false);
        }
    }
}