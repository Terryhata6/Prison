using System;
using DG.Tweening;
using Game.InGameLoot;
using UnityEngine;

namespace Game.Hero
{
    public class HeroLootTracker : MonoBehaviour
    {
        private HeroMove _heroMove;

        public void Init(HeroMove heroMove)
        {
            _heroMove = heroMove;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other)
            {
                if (other.CompareTag(tag: "Loot"))
                {
                    LootContainer lootContainer;
                    if(other.TryGetComponent<LootContainer>(out lootContainer))
                    {
                        if (lootContainer.Avaiability)
                        {
                            GetCurrency(lootContainer);
                        }
                    }
                }
            }
        }

        private void GetCurrency(LootContainer lootContainer)
        {
            lootContainer.Collect(transform);
            
            
        }
    }
}