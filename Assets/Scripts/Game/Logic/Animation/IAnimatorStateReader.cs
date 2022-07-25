using Game.Enums;
using Game.Hero;

namespace Game.Enemy
{
    public interface IAnimatorStateReader
    {
        void EnteredState(int stateHash);
        void ExitedState(int stateHash);
        AnimatorState State { get; }
    }
}