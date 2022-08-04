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
        private ISaveLoadService _saveLoadService;

        public GameLoopState(GameStateMachine stateMachine, ICoroutineRunner coroutineRunner, IUIService uiService, ISaveLoadService loadService)
        {
            _stateMachine = stateMachine;
            _coroutineRunner = coroutineRunner;
            _uiService = uiService;
            _saveLoadService = loadService;
        }

        public void Enter(HeroMove payload)
        {
            _player = payload;
            _player.OnLevelEnded += OnLevelEnded;
            _uiService.SetState(UIState.Ingame);
            if (_player.NextLevel == "Main")
            {
                _player.ActivateCopTimer();
            }
            _player.SetNextLevel("Lobby");
            _saveLoadService.SaveProgress();
            
            
        }

        private void OnLevelEnded(EndLevelData obj)
        {
            if (obj.IsLevelLobby)
            {
                _player.SetNextLevel("Main");
                _saveLoadService.SaveProgress();
                EndGame();
                Debug.Log("what");
            }
            else
            {
                _uiService.EndCaveLevelCoroutine(_coroutineRunner, obj, EndGame);
                _player.AddMoney(obj.EarnedCash);
                _saveLoadService.SaveProgress();
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