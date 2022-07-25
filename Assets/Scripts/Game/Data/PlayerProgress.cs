using System;

namespace Game.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public WorldData WorldData;
        
        public PlayerProgress(string initialLevel)
        {
            WorldData = new WorldData(initialLevel);
        }

        public PlayerData PlayerData { get; set; }
        public CurrencyData CurrencyData { get; set; }
    }
}