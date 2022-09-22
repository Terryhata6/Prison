using System;
using Game.Hero;
using Game.Infrastructure.Services;
using Game.Infrastructure.Services.SaveLoad;
using Game.Logic.EventIndicator;
using UnityEngine;

namespace Game.Logic
{
    public class CaveExit : EventContainer
    {
        public bool IsEscape = false;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (IsEscape)
                {
                    other.GetComponent<HeroMove>()?.EscapedFromCave();
                    PlayerPrefs.SetInt("HaveTileProgress", 0);
                }
                else
                    other.GetComponent<HeroMove>()?.ReturnToJail();
            }
        }

        public override void Activate(HeroMove hero)
        {
            FindObjectOfType<HeroMove>().GetComponent<HeroMove>()?.ReturnToJail();
        }

        public override void Deactivate()
        {
            
        }
    }
}