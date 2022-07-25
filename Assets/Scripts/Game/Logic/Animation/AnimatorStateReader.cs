using Game.Enemy;
using Game.Enums;
using UnityEngine;
using Game.Hero;

namespace Game.Logic
{
    public class AnimatorStateReader : MonoBehaviour, IAnimatorStateReader
    {
        private static readonly int Die = Animator.StringToHash("");
        
        private Animator _animator;

        private void Awake() => _animator = GetComponent<Animator>();

        public void PlayDeath() => _animator.SetTrigger(Die);

        public void EnteredState(int stateHash)
        {
            
        }

        public void ExitedState(int stateHash)
        {
            
        }

        public AnimatorState State { get; }
    }
}