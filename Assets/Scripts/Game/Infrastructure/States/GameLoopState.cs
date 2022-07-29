using System.Collections;
using Game.Data;
using Game.Hero;
using Game.Infrastructure.Services;
using Game.Infrastructure.Services.SaveLoad;
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
        private ISaveLoadService saveLoadService;

        public GameLoopState(GameStateMachine stateMachine, ICoroutineRunner coroutineRunner, IUIService uiService, ISaveLoadService loadService)
        {
            _stateMachine = stateMachine;
            _coroutineRunner = coroutineRunner;
            _uiService = uiService;
            saveLoadService = loadService;
        }

        public void Enter(HeroMove payload)
        {
            _player = payload;
            _player.OnLevelEnded += OnLevelEnded;
            _uiService.SetState(UIState.Ingame);
        }

        private void OnLevelEnded(EndLevelData obj)
        {
            if (obj.IsLevelLobby)
            {
                _player.SetNextLevel("Main");
                saveLoadService.SaveProgress();
                EndGame();
            }
            else
            {
                _uiService.EndCaveLevelCoroutine(_coroutineRunner, obj, EndGame);
                _player.AddMoney(obj.EarnedCash);
                saveLoadService.SaveProgress();
            }
        }

        private void EndGame()
        {
            _stateMachine.Enter<LoadProgressState>();
        }

        public void Exit()
        {
            
        }

    }
}