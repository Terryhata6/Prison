using Game.Hero;
using Game.Infrastructure.Factory;

namespace Game.Logic
{
    public interface IInit
    {
        void Init(IGameFactory factory);
    }
}