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
        
        public void FillProgress(HeroMove hero)
        {
            if (!CanFillProgress)
            {
                return;
            }

            UpdateUIIndicator(Progress);
            if (Progress < 1)
            {
                Progress += Time.deltaTime;
            }
            else
            {
                Progress = 1;
                Activate(hero);
            }
            
        }

        private void UpdateUIIndicator(float uiIndicatorFillAmount)
        {
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
            Activated = false;
        }
    }
}