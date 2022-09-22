using System;
using Game.Infrastructure.Factory;
using Game.Infrastructure.Services;
using Game.Logic;
using Game.Logic.EventIndicator;
using UnityEngine;

namespace Game.Infrastructure.Tutorial
{
    public class TutorialStarterTile : MonoBehaviour
    {
        public TileBox tile;
        public DownToCave toCaveExit;
        private void Awake()
        {
            if (PlayerPrefs.GetInt("TutorialLobbyTileSlammed", 0) < 1)
            {
                tile.Type = CurrencyType.Void;
                tile.Simple.SetActive(true);
                tile.OnSlamTile += () =>
                {
                    PlayerPrefs.SetInt("TutorialLobbyTileSlammed", 1);
                    toCaveExit.gameObject.SetActive(true);
                    Destroy(gameObject, 0);
                };
            }
            else
            {
                toCaveExit.gameObject.SetActive(true);
                Destroy(gameObject, 0);
            }
        }
    }
}