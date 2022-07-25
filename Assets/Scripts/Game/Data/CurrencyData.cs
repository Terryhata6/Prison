using System;

namespace Game.Data
{
    [Serializable]
    public class CurrencyData
    {
        public float Copper;
        public float Iron;
        public float Diamonds;
        
        public CurrencyData(float copper = 0f, float iron = 0f, float diamonds = 0f)
        {
            Copper = copper;
            Iron = iron;
            Diamonds = diamonds;
        }
    }
}