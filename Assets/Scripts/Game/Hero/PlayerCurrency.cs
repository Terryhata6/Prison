using System;
using Game.Data;
using Game.Infrastructure.Services.PersistantProgress;
using UnityEngine;

namespace Game.Hero
{
    public class PlayerCurrency : MonoBehaviour, ISavedProgress
    {
        public float Copper;
        public float Iron;
        public float Diamonds;
        

        public void LoadProgress(PlayerProgress progress)
        {
            Copper = progress.CurrencyData.Copper;
            Iron = progress.CurrencyData.Iron;
            Diamonds = progress.CurrencyData.Diamonds;
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            
        }
    }
}