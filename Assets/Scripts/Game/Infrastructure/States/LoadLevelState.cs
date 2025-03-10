using Game.Hero;
using Game.Infrastructure.Factory;
using Game.Infrastructure.Services.PersistantProgress;
using UnityEngine;

namespace Game.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IGameFactory _gameFactory;
        private readonly IPersistantProgressService _progressService;
        public string InitialPoint = "InitialPoint";
        private HeroMove _heroMove;


        public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain, IGameFactory gameFactory, IPersistantProgressService progressService)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _progressService = progressService;
        }


        public void Enter(string sceneName)
        {
            _loadingCurtain.Show();
            _gameFactory.CleanUp();
            _sceneLoader.Load(sceneName,onLoaded:OnLoaded);
        }
        
        public void Exit()
        {
            _loadingCurtain.Hide();
        }
        
        private void OnLoaded()
        {
            HeroMove player;
            InitGameWorld(out player);
            InformProgressReaders();
            _stateMachine.Enter<GameLoopState, HeroMove>(player);
        }

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
            {
                progressReader.LoadProgress(_progressService.Progress);
            }
        }

        private void InitGameWorld(out HeroMove _heroMove)   
        {
            _heroMove = _gameFactory.CreateHero(at: GameObject.FindWithTag(tag: InitialPoint));

            //Set follower to camera TODO
            _gameFactory.CreateHud();
            _gameFactory.CreateCamera(hero: _heroMove);
            _gameFactory.LoadLevelTiles();
        }
    }
}