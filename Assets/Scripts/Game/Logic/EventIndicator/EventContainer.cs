using Game.Hero;
using UnityEngine;

namespace Game.Logic.EventIndicator
{
    public abstract class EventContainer : MonoBehaviour
    {
        public abstract void Activate(HeroMove hero);

        public abstract void Deactivate();
    }
}