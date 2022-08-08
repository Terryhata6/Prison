using Game.Hero;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Logic.EventIndicator
{
    public class MapProvider : MerchantContent
    {
        public int MapPrice;
        public GameObject EnoughMoneyUi;
        public TMP_Text PriceText;
        public GameObject NotEnoughMoneyUi;

        public override void Init(HeroMove hero, Button upgradeButton)
        {
            base.Init(hero, upgradeButton);
            PriceText.text = MapPrice + "$";
            NotEnoughMoneyUi.gameObject.SetActive(false);
            EnoughMoneyUi.gameObject.SetActive(false);
            
            if (hero.Currency.Money >= MapPrice)
            {
                if (PlayerPrefs.GetInt("MapBuyed", 0) < 1)
                {
                    upgradeButton.onClick.AddListener(() => BuyingMap(heroMove:hero, upgradeButton:upgradeButton));
                    upgradeButton.interactable = true;
                    EnoughMoneyUi.SetActive(true);
                }
                else
                {
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

        private void BuyingMap(HeroMove heroMove, Button upgradeButton)
        {
            heroMove.Currency.SpendMoney(MapPrice);
            PlayerPrefs.SetInt("MapBuyed", 1);
            upgradeButton.interactable = false;
            PriceText.text = "Buyed!";
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }
    }
}