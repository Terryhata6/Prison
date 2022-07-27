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

        public void Init(IGameFactory factory)
        {
            _factory = factory;
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