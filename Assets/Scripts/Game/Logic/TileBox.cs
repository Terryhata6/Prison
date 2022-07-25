using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public interface IInit
{
    void Init();
}

public class TileBox : MonoBehaviour, IInit
{
    public float _health = 5;
    public BoxCollider _collider;

    public void Init()
    {
        
    }
    
    public void GetDamage(Action onDeath, float damage = 1f)
    {
        _health -= damage;
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
