using System;
using DG.Tweening;
using Game.Hero;
using Game.Infrastructure.Factory;
using Game.Infrastructure.Services;
using Game.Infrastructure.States;
using UnityEngine;
using static UnityEngine.PlayerPrefs;
using Random = UnityEngine.Random;

namespace Game.Logic
{
    public class TileBox : MonoBehaviour, IInit
    {
        public float _health = 5;
        public BoxCollider _collider;
        private IGameFactory _factory;
        public CurrencyType Type;

        public GameObject Simple;
        public GameObject IronGO;
        public GameObject CopperGO;
        public GameObject GemGO;
        private TileController _tileController;
        public Transform CurrentTransform;

        
        public Action OnSlamTile;

        public void SlamTile()
        {
            OnSlamTile?.Invoke();
            OnSlamTile = null;
        }

        
        
        public GameObject Init(IGameFactory factory, TileController tileController, bool useSave)
        {
            _tileController = tileController;
            _factory = factory;
            
            var randomValue = Random.Range(0, 100);

            if (useSave)
            {
                switch (GetInt(gameObject.name+"SavedType", 0))
                {
                    case 0:
                        Type = CurrencyType.Void;
                        break;
                    case 1:
                        Type = CurrencyType.Copper;
                        break;
                    case 2:
                        Type = CurrencyType.Iron;
                        break;
                    case 3:
                        Type = CurrencyType.Gem;
                        break;
                    case -1:
                        Type = CurrencyType.Void;
                        Destroy(gameObject); 
                        return null;
                }
            }
            else
            {
                RandomiseType(randomValue);
            }

            IronGO.SetActive(false);
            CopperGO.SetActive(false);
            GemGO.SetActive(false);
            Simple.SetActive(false);
            
            switch (Type)
            {
                case CurrencyType.Void:
                    Simple.SetActive(true);
                    CurrentTransform = Simple.transform;
                    break;
                case CurrencyType.Copper:
                    CopperGO.SetActive(true);
                    CurrentTransform = CopperGO.transform;
                    break;
                case CurrencyType.Iron:
                    IronGO.SetActive(true);
                    CurrentTransform = IronGO.transform;
                    break;
                case CurrencyType.Gem:
                    GemGO.SetActive(true);
                    CurrentTransform = GemGO.transform;
                    break;
                default:
                    break;
            }
            return CurrentTransform.gameObject;
        }

        private void RandomiseType(int randomValue)
        {
            if (gameObject.activeSelf == false)
                SetInt(gameObject.name + "SavedType", -1);
            if (randomValue > 90)
            {
                SetInt(gameObject.name + "SavedType", 3);
                Type = CurrencyType.Gem;
            }
            else if (randomValue > 70)
            {
                SetInt(gameObject.name + "SavedType", 2);
                Type = CurrencyType.Iron;
            }
            else if (randomValue > 40)
            {
                SetInt(gameObject.name + "SavedType", 1);
                Type = CurrencyType.Copper;
            }
            else
            {
                SetInt(gameObject.name + "SavedType", 0);
                Type = CurrencyType.Void;
            }
        }

        public void GetDamage(Action onDeath, float damage = 1f)
        {
            _health -= damage;
            int delta = 1;
            
            for (int i = 0; i < damage; i+=delta)
            {
                SpawnCurrency(Type);
            }
            if (_health < 0)
            {
                _collider.enabled = false;
            
                ShakeTile(true);
                if(_tileController)
                    _tileController.TileWasDestroyed(this);
                PlayerPrefs.SetInt(gameObject.name + "SavedType", -1);
                SlamTile();
                onDeath?.Invoke();
            }
            else
            {
                ShakeTile();
            }
        }

        private GameObject SpawnCurrency(CurrencyType currencyType)
        {
            if (currencyType == CurrencyType.Void)
            {
                return null;
            }
            GameObject currency = _factory.CreateCurrency(currencyType, transform.position, _factory.CreatedHero);
            return currency;
        }


        public void ShakeTile(bool death = false)
        {
            CurrentTransform.DOShakeScale(0.3f,0.4f,50).OnComplete(() =>
            {
                if (death)
                {
                    AllServices.Container.Single<ISoundController>().PlaySound("TileDestroyed");
                    DestroyTile();
                }
            });
        }

        private void DestroyTile()
        {
            transform.position = new Vector3(-99f, -99f, -99f);
            gameObject.SetActive(false);
        }
    }
}