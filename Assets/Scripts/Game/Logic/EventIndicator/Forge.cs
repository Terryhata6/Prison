using System;
using Game.Hero;
using Game.Logic.InGameLoot;
using MoreMountains.Tools;
using UnityEngine;

namespace Game.Logic.EventIndicator
{
    public class Forge : EventContainer
    {
        public HeroMove _hero;
        public float CopperTransferValue = 5f;
        public float IronTransferValue = 10f;
        public float GemTransferValue= 25f;

        public override void Activate(HeroMove hero)
        {
            if (!_hero)
            {
                _hero = hero;
            }
            hero.CollectCurrentInventory(LootContainerOperator);
        }

        private void LootContainerOperator(LootContainer obj)
        {
            if (!_hero)
            {
                return;
            }

            float objValue = 0f;
            switch (obj.Type)
            {
                case CurrencyType.Void:
                    break;
                case CurrencyType.Copper:
                    objValue = CopperTransferValue;
                    break;
                case CurrencyType.Iron:
                    objValue = IronTransferValue;
                    break;
                case CurrencyType.Gem:
                    objValue = GemTransferValue;
                    break;
                case CurrencyType.Unbreakable:
                    break;
                default:
                    break;
            }
            
            _hero.AddMoney(objValue);
            MMFloatingTextSpawnEvent.Trigger(0,transform.position + transform.up, objValue.ToString()+"$",transform.up, 0.1f);
            
            
        }

        public override void Deactivate()
        {
            
        }
    }
}