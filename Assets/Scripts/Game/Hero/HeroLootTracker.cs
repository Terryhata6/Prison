using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game.Infrastructure.Services;
using Game.Infrastructure.States;
using Game.Logic;
using Game.Logic.EventIndicator;
using Game.Logic.InGameLoot;
using Game.UI.Interfaces;
using UnityEngine;

namespace Game.Hero
{
    public class HeroLootTracker : MonoBehaviour
    {
        private HeroMove _heroMove;
        private Stack<LootContainer> _currencyStack = new Stack<LootContainer>();
        public Transform StackingHolder;
        public Dictionary<CurrencyType, float> CurrencyValues = new Dictionary<CurrencyType, float>();
        public int MaximumStackSize = 10;
        private IUIService _uiService;
        private bool _needRevealForge = false;
        public void Init(HeroMove heroMove, IUIService uiService, int maximumStackSize = 10)
        {
            _heroMove = heroMove;
            CurrencyValues.Add(CurrencyType.Copper, 10f);
            CurrencyValues.Add(CurrencyType.Iron, 20f);
            CurrencyValues.Add(CurrencyType.Gem, 50f);
            _uiService = uiService;
            SetMaximumInventorySize(maximumStackSize);
            UpdateInventory();
            if (PlayerPrefs.GetInt("ForgeOpen", 0) < 1)
            {
                _needRevealForge = true;
            }
        }

        public void OnTriggerStay(Collider other)
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
                            if (_currencyStack.Count < MaximumStackSize)
                            {
                                CollectCurrency(lootContainer);
                                if (_needRevealForge)
                                {
                                    Forge forge = FindObjectOfType<Forge>(true);
                                    forge.RevealForge();
                                    PlayerPrefs.SetInt("ForgeOpen", 1);
                                    _heroMove.navigationArrowToForge.ActivateNavigationTo(forge.transform);
                                    _needRevealForge = false;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CollectCurrency(LootContainer lootContainer)
        {
            _currencyStack.Push(lootContainer);
            lootContainer.CollectToBackBag(transform, AddToStuck, _currencyStack.Count);
        }

        public void AddToStuck(LootContainer container, int currentStackCount)
        {
            container.SetParentAndPosition(StackingHolder, StackingHolder.position + StackingHolder.transform.up * 0.1f *  currentStackCount);
            UpdateInventory();
            
        }

        public float GetCurrency()
        {
            float result = 0;
            while (_currencyStack.Count > 0)
            {
                var container = GetLastContainer();
                result += CurrencyValues[container.Type];
                Destroy(container.gameObject);
            }
            return result;
        }

        private LootContainer GetLastContainer()
        {
            var container = _currencyStack.Pop();
            UpdateInventory();
            return container;
        }

        public IEnumerator CollectCurrentCurrency(Action<LootContainer> containerOperator)
        {
            while (_currencyStack.Count > 0)
            {
                AllServices.Container.Single<ISoundController>().PlaySound("ThrowIngot");
                var container = GetLastContainer();
                containerOperator?.Invoke(container);
                yield return new WaitForSeconds(0.1f);
                Destroy(container.gameObject);
            }
        }

        public void UpdateInventory()
        {
            
            string currentInventory = $"{_currencyStack.Count}/{MaximumStackSize}";
            if (_currencyStack.Count == MaximumStackSize)
            {
                _uiService.UpdateUiInventory(currentInventory, Color.red);
            }
            else
            {
                _uiService.UpdateUiInventory(currentInventory, Color.white);
            }
            
        }

        public void SetMaximumInventorySize(int playerDataMaximumStackSize)
        {
            MaximumStackSize = playerDataMaximumStackSize;
        }
    }
}