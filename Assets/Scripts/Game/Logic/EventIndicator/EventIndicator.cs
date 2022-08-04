using System;
using Game.Hero;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Logic.EventIndicator
{
    public class EventIndicator : MonoBehaviour
    {
        public EventContainer Container;
        public float Progress = 0f;
        public bool CanFillProgress = true;
        public bool Activated = false;
        public Image UIIndicator;
        public float FillingTime = 1f;
        public BoxCollider Collider;
        
        public void FillProgress(HeroMove hero)
        {
            if (!CanFillProgress)
            {
                return;
            }

            UpdateUIIndicator(Progress);
            if (Progress < 1)
            {
                Progress += Time.deltaTime/FillingTime;
            }
            else
            {
                UpdateUIIndicator(Progress,false);
                Progress = 1;
                Activate(hero);
            }
            
        }

        private void UpdateUIIndicator(float uiIndicatorFillAmount, bool indicatorEnabled = true)
        {
            UIIndicator.gameObject.SetActive(indicatorEnabled);
            UIIndicator.fillAmount = uiIndicatorFillAmount;
        }

        private void Activate(HeroMove heroMove)
        {
            Container.Activate(heroMove);
            CanFillProgress = false;
            Activated = !CanFillProgress;
        }

        private void Deactivate()
        {
            Container.Deactivate();
            CanFillProgress = true;
            Activated = !CanFillProgress;
        }

        public void StopFillingProgress()
        {
            Progress = 0;
            Deactivate();
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = new Color32(30, 255, 255, 130);
            Gizmos.DrawCube(transform.position + Collider.center, Collider.size);
        }
    }
}