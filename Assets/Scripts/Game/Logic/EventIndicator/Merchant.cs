using System;
using Cinemachine;
using Game.Hero;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Logic.EventIndicator
{
    public class Merchant : EventContainer
    {
        public CinemachineVirtualCamera LocalCamera;
        public GameObject UIContent;
        public Button UpgradeButton;
        public MerchantContent Content;

        private void Awake()
        {
            DeactivateMerchantUi();
        }

        public override void Activate(HeroMove hero)
        {
            ActivateLocalCamera();
            ActivateMerchantUi();
            ActivateContent(hero);
        }

        public override void Deactivate()
        {
            DeactivateMerchantUi();
            DeactivateLocalCamera();
            DeactivateContent();
        }

        private void ActivateContent(HeroMove hero)
        {
            Content.gameObject.SetActive(true);
            Content.Init(hero, UpgradeButton);
        }

        private void DeactivateContent()
        {
            Content.Deactivate();
            Content.gameObject.SetActive(false);
        }

        private void ActivateMerchantUi()
        {
            UIContent.gameObject.SetActive(true);
        }

        private void DeactivateMerchantUi()
        {
            UIContent.gameObject.SetActive(false);
        }

        private void ActivateLocalCamera()
        {
            LocalCamera.Priority = 99;
        }

        private void DeactivateLocalCamera()
        {
            LocalCamera.Priority = 1;
        }
    }
}