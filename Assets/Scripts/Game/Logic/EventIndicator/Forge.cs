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
        public int CopperTransferValue = 5;
        public int IronTransferValue = 10;
        public int GemTransferValue= 25;

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

            int objValue = 0;
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