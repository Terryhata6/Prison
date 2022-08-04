using System;
using Game.Hero;

namespace Game.Data
{
    [Serializable]
    public class PlayerData
    {
        //TODO ProgressHere
        public PlayerData(WeaponType weaponType, int maximumStackSize, float copDelayTime)
        {
            weaponWeaponType = weaponType;
            MaximumStackSize = maximumStackSize;
            CopDelayTime = copDelayTime;
        }

        public WeaponType weaponWeaponType;
        public int MaximumStackSize;
        public float CopDelayTime;
    }
}