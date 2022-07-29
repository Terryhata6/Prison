using System;
using System.Collections.Generic;
using Game.Infrastructure.Factory;
using Game.Infrastructure.Services;
using Game.Infrastructure.Services.PersistantProgress;
using Game.Infrastructure.Services.SaveLoad;
using Game.UI.Interfaces;

//using Unity.VisualScripting;

namespace Game.Infrastructure.States
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain loadingCurtain, AllServices services, ICoroutineRunner coroutineRunner)
        {
            _states = new Dictionary<Type, IExitableState>()
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, services),
                [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader, loadingCurtain, services.Single<IGameFactory>(), services.Single<IPersistantProgressService>()),
                [typeof(GameLoopState)] = new GameLoopState(this, coroutineRunner, services.Single<IUIService>(), AllServices.Container.Single<ISaveLoadService>()),
                [typeof(LoadProgressState)] = new LoadProgressState(this, services.Single<IPersistantProgressService>(), 
                services.Single<ISaveLoadService>())
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            var state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            var state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();
            
            TState state = GetState<TState>();
            _activeState = state;
            
            return state;
        }
        
        private TState GetState<TState>() where TState : class, IExitableState =>
            _states[typeof(TState)] as TState;


        
    }
}