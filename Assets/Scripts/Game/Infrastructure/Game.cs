using Game.Infrastructure.Services;
using Game.Infrastructure.States;
using Game.Logic.Services;
using UnityEngine;

namespace Game.Infrastructure
{

    public class Game 
    {
        public GameStateMachine StateMachine;

        public Game(ICoroutineRunner coroutineRunner, LoadingCurtain loadingCurtain)
        {
            StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), loadingCurtain, AllServices.Container);
        }

    }
}
