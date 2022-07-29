using Game.Hero;

namespace Game.Logic.EventIndicator
{
    public class DownToCave : EventContainer
    {
        public override void Activate(HeroMove hero)
        {
            hero.GoToCave();
        }

        public override void Deactivate()
        {
            
        }
    }
}