using System.Collections;
using Game.Data;
using Game.Hero;
using Game.Infrastructure.Analytics;
using Game.Infrastructure.Services;
using Game.Infrastructure.Services.SaveLoad;
using Game.UI;
using Game.UI.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Infrastructure.States
{
    public class GameLoopState : IPayloadedState<HeroMove>
    {
        private readonly GameStateMachine _stateMachine;
        private HeroMove _player;
        private ICoroutineRunner _coroutineRunner;
        private IUIService _uiService;
        private ISaveLoadService _saveLoadService;
        private IAnalytics _analyticsService;

        public GameLoopState(GameStateMachine stateMachine, ICoroutineRunner coroutineRunner, IUIService uiService,
            ISaveLoadService loadService, IAnalytics analyticsService)
        {
            _stateMachine = stateMachine;
            _coroutineRunner = coroutineRunner;
            _uiService = uiService;
            _saveLoadService = loadService;
            _analyticsService = analyticsService;
        }

        public void Enter(HeroMove payload)
        {
            _player = payload;
            _player.OnLevelEnded += OnLevelEnded;
            _uiService.SetState(UIState.Ingame);
            if (SceneManager.GetActiveScene().name == "Main")
            {
                _player.InitCaveLevelStarted();
                
            }
            else
            {
                if (PlayerPrefs.GetInt("TriesPlayed", 0) < 1)
                {
                    _player.StartJoinCaveTutorial();
                }
            }
            _player.SetNextLevel("Lobby");
            _saveLoadService.SaveProgress();
            _analyticsService.OnLevelStart();
            
        }

        private void OnLevelEnded(EndLevelData obj)
        {
            if (obj.IsLevelLobby)
            {
                _player.SetNextLevel("Main");
                _saveLoadService.SaveProgress();
                EndGame();
            }
            else
            {
                
                PlayerPrefs.SetInt("TriesPlayed", PlayerPrefs.GetInt("TriesPlayed", 0) + 1);
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