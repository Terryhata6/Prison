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
        public float Money;
        private float CopMoney;
        private IUIService uiService;

        public void Start()
        {
            InitUiService();
        }

        public void LoadProgress(PlayerProgress progress)
        {
            Money = progress.CurrencyData.Money;
            CopMoney = progress.CurrencyData.CopMoney;
            UpdateUIMoney();
        }

        public void UpdateProgress(PlayerProgress progress, string currentLevel = null)
        {
            progress.CurrencyData.Money = Money;
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
            UpdateUIMoney();
        }
        
        

        public void AddCopSavings(float earnedCash)
        {
            CopMoney += earnedCash;
        }
    }
}