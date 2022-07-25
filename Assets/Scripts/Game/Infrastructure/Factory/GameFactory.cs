using System;
using System.Collections.Generic;
using Cinemachine;
using Game.Hero;
using Game.Infrastructure.AssetManagment;
using Game.Infrastructure.Services.PersistantProgress;
using Game.Logic;
using Game.Logic.Services;
using UnityEngine;

namespace Game.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssets _assets;

        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();
        
        public GameFactory(IAssets assets)
        {
            _assets = assets;
        }
        
        public GameObject CreateHero(GameObject at)
        {
            var gameObject = InstantiateRegistered(AssetPath.HeroPath, at.transform.position);

            return gameObject;
        }

        public void CreateHud()
        {
            //TODO HudLoaded
            //InstantiateRegistered(AssetPath.HudPath);
        }

        public void CreateCamera(HeroMove hero)
        {
            var gameObject = _assets.Instantiate(AssetPath.HeroCamera, at: hero.transform.position + new Vector3(0f, 5f, -3f));
            PlayerCamera worldCamera = gameObject.GetComponent<PlayerCamera>();
            worldCamera.SetupCamera(hero);
            //worldCamera.LookAt = hero.transform;

        }

        public GameObject CreateCurrency(CurrencyType currencyType, Vector3 position)
        {
            string path = "";
            switch (currencyType)
            {
                case CurrencyType.Copper:
                    path = AssetPath.CopperCurrencyPath;
                    break;
                case CurrencyType.Iron:
                    path = AssetPath.IronCurrencyPath;
                    break;
                case CurrencyType.Gem:
                    path = AssetPath.GemCurrencyPath;
                    break;
                default:
                    Debug.LogError("WrongAssetPath");
                    return null;
            }
            return _assets.Instantiate(path, position);
        }

        public void CleanUp()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }

        public IInputService CreateInputController()
        {
            return _assets.Instantiate(AssetPath.InputServicePath).GetComponent<IInputService>();
        }

        private GameObject InstantiateRegistered(string prefabPath, Vector3 position)
        {
            GameObject gameObject = _assets.Instantiate(prefabPath, position);
            
            RegisterProgressWatchers(gameObject);
            
            return gameObject;
        }
        private GameObject InstantiateRegistered(string prefabPath)
        {
            GameObject gameObject = _assets.Instantiate(prefabPath);
            
            RegisterProgressWatchers(gameObject);
            
            return gameObject;
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
            {
                Register(progressReader);
            }
        }

        private void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
                ProgressWriters.Add(progressWriter);
            ProgressReaders.Add(progressReader);
        }
    }
}