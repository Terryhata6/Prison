using System;
using Game.Hero;
using Game.Infrastructure.Services;
using Game.Infrastructure.Services.SaveLoad;
using UnityEngine;

namespace Game.Logic
{
    public class CaveExit : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<HeroMove>()?.EscapedFromCave();
            }
        }
    }
}