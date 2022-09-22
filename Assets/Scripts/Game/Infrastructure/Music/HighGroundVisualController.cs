using System;
using System.Collections.Generic;
using DG.DemiLib.Attributes;
using UnityEngine;

namespace Game.Infrastructure.Music
{
    [DeScriptExecutionOrder(-1)]
    public class HighGroundVisualController : MonoBehaviour, IHighGroundVisualController
    {
        public List<Material> MaterialsPool = new List<Material>();
        public MB2_TextureBakeResults TextureBakeResults;
        public Material CurrentMaterial;

        private Dictionary<string, Material> _fastAccess = new Dictionary<string, Material>();
        private BakedMeshTraker CurrentTracker;

        private void Awake()
        {
            _fastAccess.Clear();
            foreach (var material in MaterialsPool)
            {
                _fastAccess.Add(material.name, material);
            }

            CurrentMaterial = TextureBakeResults.resultMaterials[2].combinedMaterial;

            if (PlayerPrefs.GetString("CurrentMaterial", "") == "")
            {
                PlayerPrefs.SetString("CurrentMaterial", TextureBakeResults.resultMaterials[2].combinedMaterial.name);
            }
            else
            {
                SetCurrentHighGroundMaterial(_fastAccess[PlayerPrefs.GetString("CurrentMaterial", "")]);
            }
        }

        public void ChangeMaterialToNextInList()
        {
            int index = -1;
            for (int i = 0; i < MaterialsPool.Count; i++)
            {
                if (MaterialsPool[i].name == CurrentMaterial.name)
                {
                    index = i;
                    break;
                }
            }

            if (index < 0)
            {
                SetCurrentHighGroundMaterial(MaterialsPool[0]);
            }
            else
            {
                if (index + 1 >= MaterialsPool.Count)
                {
                    SetCurrentHighGroundMaterial(MaterialsPool[0]);
                }
                else
                {
                    SetCurrentHighGroundMaterial(MaterialsPool[index + 1]);
                }
            }
        }

        private void SetCurrentHighGroundMaterial(Material newMaterial)
        {
            if (CurrentTracker == null)
            {
                CurrentTracker = FindObjectOfType<BakedMeshTraker>();
            }
            if (CurrentTracker != null)
            {
                var materials = CurrentTracker.GetRenderer.materials;
                materials[2] = newMaterial;
                CurrentTracker.GetRenderer.materials = materials;
            }
            CurrentMaterial = TextureBakeResults.resultMaterials[2].combinedMaterial = newMaterial;
            PlayerPrefs.SetString("CurrentMaterial", newMaterial.name);
        }
    }
}