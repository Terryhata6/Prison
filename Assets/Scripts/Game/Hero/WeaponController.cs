using System;
using System.Collections.Generic;
using Game.Infrastructure.Services;
using Game.Infrastructure.Services.SaveLoad;
using UnityEngine;

namespace Game.Hero
{
    public class WeaponController : MonoBehaviour
    {
        public PlayerWeapon CurrentWeapon;
        public List<PlayerWeapon> Weapons;
        
        private void Awake()
        {
            foreach (PlayerWeapon playerWeapon in Weapons)
            {
                playerWeapon.gameObject.SetActive(false);
            }
            
        }

        public PlayerWeapon GetWeaponByType(WeaponType playerDataWeaponType)
        {
            foreach (var weapon in Weapons)
            {
                if (weapon.Type == playerDataWeaponType)
                {
                    return weapon;
                }
            }
            return null;
        }

        public void SetCurrentWeapon(WeaponType playerDataWeaponType, HeroMove heroMove)
        {
            foreach (var VARIABLE in Weapons)
            {
                VARIABLE.gameObject.SetActive(false);
            }
            CurrentWeapon = GetWeaponByType(playerDataWeaponType);
            CurrentWeapon.gameObject.SetActive(true);
            heroMove.UpdateWeaponParams(CurrentWeapon.AttackSpeed);
            AllServices.Container.Single<ISaveLoadService>().SaveProgress();
        }
        
        
    }
}