using Game.Hero;
using Game.Infrastructure.Services;
using Game.Infrastructure.States;

namespace Game.Logic.EventIndicator
{
    public class DownToCave : EventContainer
    {
        public override void Activate(HeroMove hero)
        {
            AllServices.Container.Single<ISoundController>().PlaySound("JumpInCave");
            hero.GoToCave();
        }

        public override void Deactivate()
        {
            
        }
    }
}