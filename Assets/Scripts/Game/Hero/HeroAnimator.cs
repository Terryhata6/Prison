using System;
using Game.Enemy;
using UnityEngine;
using AnimatorState = Game.Enums.AnimatorState;

namespace Game.Hero
{
    public class HeroAnimator : MonoBehaviour, IAnimatorStateReader
    {
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int AttackSpeed = Animator.StringToHash("AttackSpeed");
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int Victory = Animator.StringToHash("Victory");
        
        private readonly int _idleStateHash = Animator.StringToHash("idle");
        private readonly int _attackStateHash = Animator.StringToHash("attack");
        private readonly int _walkingStateHash = Animator.StringToHash("move");

        private Animator _animator;

        public event Action<AnimatorState> StateEntered;
        public event Action<AnimatorState> StateExited;

        [SerializeField] private AnimatorState _state;
        public AnimatorState State { get => _state;
            private set => _state = value;
        }

        private void Awake() =>
            _animator = GetComponent<Animator>();
       
        public void Move(float speed)
        {
            //StopAttack();
            _animator.SetBool(IsMoving, true);
            _animator.SetFloat(Speed,speed);
        }

        public void StopMoving() => _animator.SetBool(IsMoving, false);
        public void PlayAttack() => _animator.SetBool(Attack, true);
        public void StopAttack() => _animator.SetBool(Attack, false);
        public void PlayVictory() => _animator.SetBool(Victory, true);
        public void StopVictory() => _animator.SetBool(Victory, true);
        public void SetAttackSpeed(float value = 1f) => _animator.SetFloat(AttackSpeed, value);

        public void EnteredState(int stateHash)
        {
            State = StateFor(stateHash);
            StateEntered?.Invoke(State);
        }

        public void ExitedState(int stateHash)
        {
            StateExited?.Invoke(StateFor(stateHash));
        }

        private AnimatorState StateFor(int stateHash)
        {
            AnimatorState state;
            if (stateHash == _idleStateHash)
                state = AnimatorState.Idle;
            else if (stateHash == _attackStateHash) 
                state = AnimatorState.Attack;
            else if (stateHash == _walkingStateHash)
                state = AnimatorState.Moving;
            else
                state = AnimatorState.Unknown;
            return state;
        }
    }
}