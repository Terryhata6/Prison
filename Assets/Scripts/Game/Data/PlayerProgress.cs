using System;

namespace Game.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public WorldData WorldData;
        public PlayerData PlayerData;
        public CurrencyData CurrencyData;



        public PlayerProgress(string initialLevel, PlayerData playerData, CurrencyData currencyData)
        {
            WorldData = new WorldData(initialLevel);
            PlayerData = playerData;
            CurrencyData = currencyData;
        }
    }
}