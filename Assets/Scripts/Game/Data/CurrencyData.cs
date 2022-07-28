using System;

namespace Game.Data
{
    [Serializable]
    public class CurrencyData
    {
        public float Money;
        public float CopMoney;


        public CurrencyData(float money = 0f, float copMoney = 0f)
        {
            Money = money;
            CopMoney = copMoney;
        }
    }
}