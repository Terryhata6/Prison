using System;
using System.Collections.Generic;
using Game.Hero;
using UnityEngine;

namespace Game.Infrastructure.Lobby
{
    public class LobbyController : MonoBehaviour
    {
        public float CurrentMoney;
        public List<ProgressDependencyObject> _objects;

        private void Start()
        {
            CurrentMoney = PlayerPrefs.GetFloat("MoneyKey");
            
            foreach (var obj in _objects)
            {
                if (obj.MoneyCount > CurrentMoney && PlayerPrefs.GetInt(obj.gameObject.name + "IsOpened", 0) == 0)
                {
                    obj.gameObject.SetActive(false);
                }
                else 
                {
                    PlayerPrefs.SetInt(obj.gameObject.name + "IsOpened", 1);
                }
            }
        }
    }

    [Serializable]
    public struct ProgressDependencyObject
    {
        public float MoneyCount;
        public GameObject gameObject;
    }
}