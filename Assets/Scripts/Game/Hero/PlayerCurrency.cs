using System;
using Game.Data;
using Game.Infrastructure.Services.PersistantProgress;
using UnityEngine;

namespace Game.Hero
{
    public class PlayerCurrency : MonoBehaviour, ISavedProgress
    {
        public float Money;
        private float CopMoney;


        public void LoadProgress(PlayerProgress progress)
        {
            Money = progress.CurrencyData.Money;
            CopMoney = progress.CurrencyData.CopMoney;
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.CurrencyData.Money = Money;
        }

        public void AddCurrency(float value)
        {
            Money += value;
        }

        public void AddCopSavings(float earnedCash)
        {
            CopMoney += earnedCash;
        }
    }
}