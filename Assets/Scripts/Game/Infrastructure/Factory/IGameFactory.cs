using System.Collections.Generic;
using Game.Hero;
using Game.Infrastructure.Services;
using Game.Infrastructure.Services.PersistantProgress;
using Game.Logic.Services;
using UnityEngine;

namespace Game.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        GameObject CreateHero(GameObject at);
        void CreateHud();
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        void CleanUp();
        IInputService CreateInputController();
        void CreateCamera(HeroMove hero);
    }
}