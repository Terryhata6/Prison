using Game.Hero;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Logic.EventIndicator
{
    public abstract class MerchantContent : MonoBehaviour
    {
        public virtual void Init(HeroMove hero, Button upgradeButton)
        {
            
        }

        public virtual void Deactivate()
        {
            
        }
    }
}