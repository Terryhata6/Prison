using System;
using Game.Hero;

namespace Game.Data
{
    [Serializable]
    public class PlayerData
    {
        //TODO ProgressHere
        public PlayerData(WeaponType type = WeaponType.Otvertka)
        {
            WeaponType = type;
        }

        public WeaponType WeaponType;
    }
}