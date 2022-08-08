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

        [Header("Baking")] public List<GameObject> RenderersToBake = new List<GameObject>();
        public MB3_MeshBaker baker;
        
        
        private void Awake()
        {
            RenderersToBake.Clear();
            _gameFactory = AllServices.Container.Single<IGameFactory>();
            if (_tiles.Count < 1)
                _tiles.AddRange(GetComponentsInChildren<TileBox>());
            foreach (TileBox tileBox in _tiles)
            {
                AddToBakingList(tileBox.Init(_gameFactory, this));
            }

            baker.AddDeleteGameObjects(RenderersToBake.ToArray(), null, true);
            baker.Apply();
            
        }

        public void TileWasDestroyed(TileBox tileBox)
        {
            _pathfinder.Scan();
        }

        public void AddToBakingList(GameObject box)
        {
            RenderersToBake.Add(box);
        }
    }
}