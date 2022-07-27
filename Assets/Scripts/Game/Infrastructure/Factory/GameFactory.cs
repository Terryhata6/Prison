using System;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using Game.Hero;
using Game.Infrastructure.AssetManagment;
using Game.Infrastructure.Services.PersistantProgress;
using Game.Logic;
using Game.Logic.Services;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssets _assets;
        private HeroMove currentHero;

        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();
        public HeroMove CreatedHero { get => currentHero;  }

        public GameFactory(IAssets assets)
        {
            _assets = assets;
        }
        
        public HeroMove CreateHero(GameObject at)
        {
            currentHero = InstantiateRegistered(AssetPath.HeroPath, at.transform.position)
                .GetComponentInChildren<HeroMove>();

            return currentHero;
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

        public GameObject CreateCurrency(CurrencyType currencyType, Vector3 position, HeroMove heroMove)
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

            var currency = _assets.Instantiate(path, position);
            currency.transform.DORotate(new Vector3(0, Random.Range(360f,540f), 0), 0.5f);
            currency.transform.DOJump(GetNewCurrencyPosition(heroMove), 1.5f,1,0.3f);
            return currency;
        }

        private static Vector3 GetNewCurrencyPosition(HeroMove heroMove)
        {
            return heroMove.transform.position + Vector3.forward * Random.Range(-0.5f,0.5f) + Vector3.right * Random.Range(-0.5f,0.5f);
        }

        public void CreateTiles()
        {
            
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