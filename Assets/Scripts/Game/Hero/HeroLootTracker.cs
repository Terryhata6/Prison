using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game.Logic;
using Game.Logic.InGameLoot;
using UnityEngine;

namespace Game.Hero
{
    public class HeroLootTracker : MonoBehaviour
    {
        private HeroMove _heroMove;
        private Stack<LootContainer> _currencyStack = new Stack<LootContainer>();
        public Transform StackingHolder;
        public Dictionary<CurrencyType, float> CurrencyValues = new Dictionary<CurrencyType, float>();

        public void Init(HeroMove heroMove)
        {
            _heroMove = heroMove;
            CurrencyValues.Add(CurrencyType.Copper, 10f);
            CurrencyValues.Add(CurrencyType.Iron, 20f);
            CurrencyValues.Add(CurrencyType.Gem, 50f);
            
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
                            CollectCurrency(lootContainer);
                        }
                    }
                }
            }
        }

        private void CollectCurrency(LootContainer lootContainer) => lootContainer.Collect(transform, AddToStuck);

        public void AddToStuck(LootContainer container)
        {
            _currencyStack.Push(container);
            container.SetParentAndPosition(StackingHolder, StackingHolder.position + StackingHolder.transform.up * 0.1f *  _currencyStack.Count);
        }

        public float GetCurrency()
        {
            float result = 0;
            while (_currencyStack.Count > 0)
            {
                var container =_currencyStack.Pop();
                result += CurrencyValues[container.Type];
                Destroy(container.gameObject);
            }
            return result;
        }
    }
}