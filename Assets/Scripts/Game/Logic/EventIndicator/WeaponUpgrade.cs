using System;
using Game.Hero;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Logic.EventIndicator
{
    public class WeaponUpgrade : MerchantContent
    {
        public GameObject Otvertka;
        public GameObject RightSpoon;
        public GameObject LeftSpoon;
        public GameObject RightKirka;
        public GameObject MidKirka;
        public Button Button;
        public TMP_Text PriceText;

        public Image Arrow;
        [Header("Prices")]public int SpoonPrice;
        public int PickaxePrice;
        
        public override void Init(HeroMove hero, Button upgradeButton)
        {
            base.Init(hero, upgradeButton);
            Button = upgradeButton;
            Reset();
            
            
            switch (hero.WeaponController.CurrentWeapon.Type)
            {
                case WeaponType.Otvertka:
                {
                    PriceText.gameObject.SetActive(true);
                    PriceText.text = $"{SpoonPrice}$";
                    Otvertka.gameObject.SetActive(true);
                    RightSpoon.gameObject.SetActive(true);
                    Arrow.gameObject.SetActive(true);
                    if (hero.Currency.Money >= SpoonPrice)
                    {
                        upgradeButton.interactable = true;
                        PriceText.color = Color.green;
                        upgradeButton.onClick.AddListener(() =>
                        {
                            hero.SpendMoney(SpoonPrice);
                            UpgradeHeroWeapon(hero, WeaponType.Spoon, SpoonPrice);
                        });
                    }
                    else
                    {
                        upgradeButton.interactable = false;
                        PriceText.color = Color.red;
                    }
                    
                    break;
                }
                case WeaponType.Spoon:
                {
                    PriceText.gameObject.SetActive(true);
                    PriceText.text = $"{PickaxePrice}$";
                    LeftSpoon.gameObject.SetActive(true);
                    RightKirka.gameObject.SetActive(true);
                    Arrow.gameObject.SetActive(true);
                    if (hero.Currency.Money >= PickaxePrice)
                    {
                        upgradeButton.interactable = true;
                        PriceText.color = Color.green;
                        upgradeButton.onClick.AddListener(() => UpgradeHeroWeapon(hero, WeaponType.Pickaxe, PickaxePrice));
                    }
                    else
                    {
                        upgradeButton.interactable = false;
                        PriceText.color = Color.red;
                    }
                    break;
                }
                case WeaponType.Pickaxe:
                {
                    Arrow.gameObject.SetActive(false);
                    MidKirka.gameObject.SetActive(true);
                    upgradeButton.gameObject.SetActive(false);
                    break;
                }
                default:
                    break;
            }
        }

        private void Reset()
        {
            if(Button != null)
                Button.onClick.RemoveAllListeners();
            Otvertka.gameObject.SetActive(false);
            RightSpoon.gameObject.SetActive(false);
            LeftSpoon.gameObject.SetActive(false);
            RightKirka.gameObject.SetActive(false);
            MidKirka.gameObject.SetActive(false);
            PriceText.gameObject.SetActive(false);
        }

        public override void Deactivate()
        {
            base.Deactivate();
            
        }

        public void UpgradeHeroWeapon(HeroMove hero, WeaponType type, int price)
        {
            hero.ActivateNewWeapon(type, price);
            Init(hero, Button);
        }
    }
}