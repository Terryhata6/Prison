using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game.Infrastructure.Factory;
using Game.Infrastructure.Services;
using Game.Logic;
using UnityEngine;

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
        SpawnCurrency(Type);
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

    private void SpawnCurrency(CurrencyType currencyType)
    {
        _factory.CreateCurrency(currencyType, transform.position);
        
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