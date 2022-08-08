using Game.Hero;
using Game.Infrastructure.Factory;
using UnityEngine;

namespace Game.Logic
{
    public interface IInit
    {
        GameObject Init(IGameFactory factory, TileController controller);
    }
}