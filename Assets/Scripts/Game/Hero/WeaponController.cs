using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Hero
{
    public class WeaponController : MonoBehaviour
    {
        public PlayerWeapon CurrentWeapon;
        public List<PlayerWeapon> _weapons;
        
        private void Awake()
        {
            foreach (PlayerWeapon playerWeapon in _weapons)
            {
                playerWeapon.gameObject.SetActive(false);
            }
            
        }

        public PlayerWeapon GetWeaponByType(WeaponType playerDataWeaponType)
        {
            if(CurrentWeapon.Type == playerDataWeaponType)
                return CurrentWeapon;
            else
            {
                return CurrentWeapon;
            }
        }

        public void SetCurrentWeapon(WeaponType playerDataWeaponType, HeroMove heroMove)
        {
            CurrentWeapon = GetWeaponByType(playerDataWeaponType);
            CurrentWeapon.gameObject.SetActive(true);
            heroMove.UpdateWeaponParams(CurrentWeapon.AttackSpeed);
        }
        
        
    }
}