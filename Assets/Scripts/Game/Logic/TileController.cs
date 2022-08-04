using System;
using System.Collections.Generic;
using Game.Infrastructure.Factory;
using Game.Infrastructure.Services;
using UnityEngine;

namespace Game.Logic
{
    public class TileController : MonoBehaviour
    {
        public List<TileBox> _tiles = new List<TileBox>();
        private IGameFactory _gameFactory;
        public AstarPath _pathfinder;

        private void Awake()
        {
            _gameFactory = AllServices.Container.Single<IGameFactory>();
            if (_tiles.Count < 1)
                _tiles.AddRange(GetComponentsInChildren<TileBox>());
            foreach (TileBox tileBox in _tiles)
            {
                tileBox.Init(_gameFactory, this);
            }
        }

        public void TileWasDestroyed(TileBox tileBox)
        {
            _pathfinder.Scan();
        }
    }
}