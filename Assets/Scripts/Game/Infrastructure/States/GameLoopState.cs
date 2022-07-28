using System.Collections;
using Game.Data;
using Game.Hero;
using Game.Infrastructure.Services;
using Game.UI;
using Game.UI.Interfaces;
using UnityEngine;

namespace Game.Infrastructure.States
{
    public class GameLoopState : IPayloadedState<HeroMove>
    {
        private readonly GameStateMachine _stateMachine;
        private HeroMove _player;
        private ICoroutineRunner _coroutineRunner;
        private IUIService _uiService;

        public GameLoopState(GameStateMachine stateMachine, ICoroutineRunner coroutineRunner, IUIService uiService)
        {
            _stateMachine = stateMachine;
            _coroutineRunner = coroutineRunner;
            _uiService = uiService;
        }

        public void Enter(HeroMove payload)
        {
            _player = payload;
            _player.OnLevelEnded += OnLevelEnded;
            _uiService.SetState(UIState.Ingame);
        }

        private void OnLevelEnded(EndLevelData obj)
        {
            _coroutineRunner.StartCoroutine(_uiService.EndLevelCoroutine(obj, Exit));
            
        }
        
        
        

        public void Exit()
        {
            _stateMachine.Enter<LoadProgressState>();
        }

    }
}