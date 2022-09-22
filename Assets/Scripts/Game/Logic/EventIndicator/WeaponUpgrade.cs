using System;
using Game.Hero;
using Game.Infrastructure.GameEvents;
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
        public Button RewardedButton;
        public TMP_Text PriceText;
        public HeroMove Hero;

        public Image Arrow;
        [Header("Prices")] public int SpoonPrice;
        public int PickaxePrice;

        public override void Init(HeroMove hero, Button upgradeButton, Button rewardedButton = null)
        {
            base.Init(hero, upgradeButton);
            Button = upgradeButton;
            RewardedButton = rewardedButton;
            Hero = hero;
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
                        upgradeButton.onClick.RemoveAllListeners();
                        upgradeButton.onClick.AddListener(() =>
                        {
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
                        upgradeButton.onClick.RemoveAllListeners();
                        upgradeButton.onClick.AddListener(() =>
                            UpgradeHeroWeapon(hero, WeaponType.Pickaxe, PickaxePrice));
                    }
                    else
                    {
                        upgradeButton.interactable = false;
                        if (RewardedButton != null)
                        {
                            if (GameEvents.RewardedIsReady)
                            {
                                RewardedButton.gameObject.SetActive(true);
                                RewardedButton.onClick.RemoveAllListeners();
                                RewardedButton.onClick.AddListener(() =>
                                {
                                    GameEvents.Instance.RewardedRequest(
                                        () => UpgradeHeroWeapon(hero, WeaponType.Pickaxe, 0), "merchant");
                                });
                            }
                            else
                            {
                                GameEvents.Instance.OnRewardedReadyStateChange += AdvertisementUpgrade;
                            }
                        }

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

        private void AdvertisementUpgrade(bool value)
        {
            if (value)
            {
                RewardedButton.gameObject.SetActive(true);
                RewardedButton.onClick.RemoveAllListeners();
                RewardedButton.onClick.AddListener(() =>
                {
                    GameEvents.Instance.RewardedRequest(
                        () => UpgradeHeroWeapon(Hero, WeaponType.Spoon, 0),
                        "merchant");
                });
            }
            else
            {
                RewardedButton.gameObject.SetActive(false);
            }
        }

        private void Reset()
        {
            if (Button != null)
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
            GameEvents.Instance.OnRewardedReadyStateChange -= AdvertisementUpgrade;
        }

        public void UpgradeHeroWeapon(HeroMove hero, WeaponType type, int price)
        {
            hero.ActivateNewWeapon(type, price);
            Init(hero, Button, RewardedButton);
        }
    }
}