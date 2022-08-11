using System;
using Game.Data;
using Game.Infrastructure.Services;
using Game.Infrastructure.Services.PersistantProgress;
using Game.UI.Interfaces;
using UnityEngine;

namespace Game.Hero
{
    public class PlayerCurrency : MonoBehaviour, ISavedProgress
    {
        public const string MoneyKey = "MoneyKey";
        public const string CopMoneyKey = "CopMoney";
        public float Money = 0;
        private float CopMoney;
        private IUIService uiService;

        public void Start()
        {
            InitUiService();
            LoadProgress();
            
        }

        public void LoadProgress(PlayerProgress progress)
        {
            //MoneyKey = progress.CurrencyData.MoneyKey;
            //CopMoney = progress.CurrencyData.CopMoney;
            LoadProgress();
            Debug.Log("CurrencyProgress Loaded");
        }

        private void LoadProgress()
        {
            Money = PlayerPrefs.GetFloat(MoneyKey, 0);
            CopMoney = PlayerPrefs.GetFloat(CopMoneyKey, 0);
            UpdateUIMoney();
        }

        public void UpdateProgress(PlayerProgress progress, string currentLevel = null)
        {
            //progress.CurrencyData.Money = Money;
            //PlayerPrefs.SetFloat(MoneyKey, Money);
            //PlayerPrefs.SetFloat(CopMoneyKey, CopMoney);
            //Debug.Log("SaveCurrencyProgress");
        }

        public void UpdateUIMoney()
        {
            InitUiService();
            uiService.UpdateUIMoney(Money);
        }

        private void InitUiService()
        {
            if (uiService == null)
            {
                uiService = AllServices.Container.Single<IUIService>();
                
            }
        }

        public void AddCurrency(float value)
        {
            Money += value;
            PlayerPrefs.SetFloat(MoneyKey, Money);
            UpdateUIMoney();
        }

        public void SpendMoney(int spoonPrice)
        {
            Money -= spoonPrice;
            PlayerPrefs.SetFloat(MoneyKey, Money);
            UpdateUIMoney();
        }

        public void AddCopSavings(float earnedCash)
        {
            CopMoney += earnedCash;
            PlayerPrefs.SetFloat(CopMoneyKey, CopMoney);
        }

        
    }
}