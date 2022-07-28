using System;
using DG.Tweening;
using Game.Hero;
using Game.Infrastructure.Factory;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Logic
{
    public class TileBox : MonoBehaviour, IInit
    {
        public float _health = 5;
        public BoxCollider _collider;
        private IGameFactory _factory;
        public CurrencyType Type;

        public GameObject IronGO;
        public GameObject CopperGO;
        public GameObject GemGO;
        
        public void Init(IGameFactory factory)
        {
            _factory = factory;
            var randomValue = Random.Range(0, 100);
            if (randomValue > 90)
                Type = CurrencyType.Gem;
            else if (randomValue > 70)
                Type = CurrencyType.Iron;
            else if(randomValue > 40)
                Type = CurrencyType.Copper;
            else
                Type = CurrencyType.Void;
            

            IronGO.SetActive(false);
            CopperGO.SetActive(false);
            GemGO.SetActive(false);
            switch (Type)
            {
                case CurrencyType.Void:
                    break;
                case CurrencyType.Copper:
                    CopperGO.SetActive(true);
                    break;
                case CurrencyType.Iron:
                    IronGO.SetActive(true);
                    break;
                case CurrencyType.Gem:
                    GemGO.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    
        public void GetDamage(Action onDeath, float damage = 1f)
        {
            _health -= damage;
            for (int i = 0; i < damage; i++)
            {
                SpawnCurrency(Type);
            }
            if (_health < 0)
            {
                _collider.enabled = false;
            
                ShakeTile(true);
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
            transform.DOShakeScale(0.3f,0.4f,50).OnComplete(() =>
            {
                if (death)
                {
                    Destroy(this.gameObject);
                }
            });
        }

    }
}