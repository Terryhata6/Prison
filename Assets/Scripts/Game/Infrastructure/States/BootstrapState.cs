using Game.Infrastructure.Analytics;
using Game.Infrastructure.AssetManagment;
using Game.Infrastructure.Factory;
using Game.Infrastructure.Music;
using Game.Infrastructure.Particles;
using Game.Infrastructure.Services;
using Game.Infrastructure.Services.PersistantProgress;
using Game.Infrastructure.Services.SaveLoad;
using Game.Infrastructure.Tutorial;
using Game.Logic.Services;
using Game.UI;
using Game.UI.Interfaces;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string Initial = "Initial";
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private AllServices _services;

        public BootstrapState(GameStateMachine stateMachine,SceneLoader sceneLoader, AllServices services)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _services = services;
            
            RegisterServices();
        }

        public void Enter()
        {
            _sceneLoader.Load(Initial, onLoaded:EnterLoadLevel);
        }

        private void EnterLoadLevel()
        {
            _stateMachine.Enter<LoadProgressState>();
        }

        private void RegisterServices()
        {
            _services.RegisterSingle<IAssets>(new AssetsProvider());
            _services.RegisterSingle<IPersistantProgressService>(new PersistantProgressService());
            _services.RegisterSingle<LevelCreatorService>(new LevelCreatorService(_services.Single<IAssets>()));
            _services.RegisterSingle<IGameFactory>(new GameFactory(_services.Single<IAssets>()));
            _services.RegisterSingle<IInputService>(SetupInputService()); 
            _services.RegisterSingle<ISaveLoadService>(new SaveLoadService(_services.Single<IPersistantProgressService>(), _services.Single<IGameFactory>()));
            _services.RegisterSingle<IUIService>(new UIService(_services.Single<IGameFactory>()));
            _services.RegisterSingle<IParticlesController>(SetupParticleController());
            _services.RegisterSingle<ITutorial>(SetupTutorial());
            _services.RegisterSingle<IAnalytics>(SetupAnalytics());
            _services.RegisterSingle<ISoundController>(SetupSoundController());
            _services.RegisterSingle<IHighGroundVisualController>(SetupHighGroundController());
            
        }

        private IHighGroundVisualController SetupHighGroundController() => GameObject.FindObjectOfType<HighGroundVisualController>();

        private ISoundController SetupSoundController()
        {
            var soundController = GameObject.FindObjectOfType<SoundController>();
            return soundController;
        }

        private IAnalytics SetupAnalytics()
        {
            var analytics = GameObject.FindObjectOfType<Analytics.Analytics>();
            if (analytics != null)
                return analytics;
            return _services.Single<IGameFactory>().CreateAnalytics();
        }

        private ITutorial SetupTutorial()
        {
            return _services.Single<IGameFactory>().CreateTutorial();
        }

        private IParticlesController SetupParticleController()
        {
            return _services.Single<IGameFactory>().CreateParticleController();
        }

        public void Exit()
        {
            
        }

        public IInputService SetupInputService()
        {
            return _services.Single<IGameFactory>().CreateInputController();
        }
    }
}