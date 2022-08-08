using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Infrastructure.Lobby
{
    public class LobbyController : MonoBehaviour
    {
        public int CurrentTries;
        public List<ProgressDependencyObject> _objects;

        private void Awake()
        {
            CurrentTries = PlayerPrefs.GetInt("TriesPlayed", 0);
            foreach (var obj in _objects)
            {
                if (obj.TryNumber > CurrentTries)
                    obj.gameObject.SetActive(false);
                else if (obj.TryNumber == CurrentTries)
                {
                    if (obj.gameObject.TryGetComponent<RevealMerchant>(out var revealMerchant))
                    {
                        revealMerchant.Reveal();
                    }
                }
            }
        }
    }

    [Serializable]
    public struct ProgressDependencyObject
    {
        public int TryNumber;
        public GameObject gameObject;
    }
}