using Game.Infrastructure.AssetManagment;
using Game.Infrastructure.Factory;
using Game.Infrastructure.Services;
using Game.Infrastructure.Services.PersistantProgress;
using Game.Infrastructure.Services.SaveLoad;
using Game.Logic.Services;

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
            _services.RegisterSingle<IGameFactory>(new GameFactory(_services.Single<IAssets>()));
            _services.RegisterSingle<IInputService>(SetupInputService()); 
            _services.RegisterSingle<ISaveLoadService>(new SaveLoadService(_services.Single<IPersistantProgressService>(), _services.Single<IGameFactory>()));
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