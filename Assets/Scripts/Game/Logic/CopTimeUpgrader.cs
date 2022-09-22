using Game.Hero;
using Game.Logic.EventIndicator;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Logic
{
    public class CopTimeUpgrader : MerchantContent
    {
        public int NextUpgradePrice;
        public GameObject EnoughMoneyUi;
        public GameObject NotEnoughMoneyUi;

        public TMP_Text CurrentTime;
        public TMP_Text UpgradePrice;
        public TMP_Text NextTime;
        public float TimeUpgradeValue = 20;

        public override void Init(HeroMove hero, Button upgradeButton, Button rewardedButton)
        {
            base.Init(hero, upgradeButton, rewardedButton);
            
            NotEnoughMoneyUi.gameObject.SetActive(false);
            EnoughMoneyUi.gameObject.SetActive(false);
        
            int currentCopTime = Mathf.RoundToInt(hero.MaximumCopTimer);
            NextUpgradePrice = currentCopTime * (2 + currentCopTime / 10);
            CurrentTime.text = currentCopTime + " sec";
            NextTime.text = (currentCopTime + TimeUpgradeValue) + " sec";
            UpgradePrice.text = NextUpgradePrice + "$";
            
            upgradeButton.onClick.RemoveAllListeners();

            if (hero.Currency.Money >= NextUpgradePrice)
            {
                if ((currentCopTime) < 300)
                {
                    upgradeButton.onClick.RemoveAllListeners();
                    upgradeButton.onClick.AddListener(() => UpgradeCopTime(heroMove:hero, upgradeButton:upgradeButton, NextUpgradePrice));
                    upgradeButton.interactable = true;
                    EnoughMoneyUi.SetActive(true);
                }
                else
                {
                    CurrentTime.text = currentCopTime + " sec";
                    NextTime.text = "";
                    NextUpgradePrice = 9999;
                    UpgradePrice.text = "";
                    
                    upgradeButton.interactable = false;
                    EnoughMoneyUi.SetActive(true);
                }
            }
            else
            {
                upgradeButton.interactable = false;
                NotEnoughMoneyUi.SetActive(true);
            }
        }

        private void UpgradeCopTime(HeroMove heroMove, Button upgradeButton, int price)
        {
            heroMove.UpgradeCopDelayTimer(TimeUpgradeValue, price);
            
            Init(heroMove, upgradeButton: upgradeButton, rewardedButton:null);
        }

        public override void Deactivate()
        {
            
        }
    }
}
