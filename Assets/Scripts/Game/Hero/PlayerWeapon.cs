using System;
using Game.Logic;
using UnityEngine;

namespace Game.Hero
{
    public class PlayerWeapon : MonoBehaviour
    {
        public WeaponType Type;
        public float AttackSpeed  = 1f;
        public void ActivateHitCollider()
        {
            
        }

    }

    public enum WeaponType
    {
        Spoon,
        Pickaxe,
        Otvertka
    }
}