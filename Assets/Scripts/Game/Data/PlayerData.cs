using System;
using Game.Hero;

namespace Game.Data
{
    [Serializable]
    public class PlayerData
    {
        public PlayerData(WeaponType type)
        {
            WeaponType = type;
        }

        public WeaponType WeaponType;
    }
}