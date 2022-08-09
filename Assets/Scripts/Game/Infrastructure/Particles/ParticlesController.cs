using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Infrastructure.Particles
{
    public class ParticlesController : MonoBehaviour, IParticlesController
    {
        public List<Particles> Particles = new List<Particles>();
        [Range(1,10)]public int PoolAmount = 5;

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            foreach (var container in Particles)
            {
                for (int i = 0; i < PoolAmount; i++)
                {
                    var instantiate = Instantiate(container.Example, new Vector3(-99f,-99f,-99f),Quaternion.identity,transform);
                    var system = instantiate.GetComponent<ParticleSystem>();
                    container.ParticlesContainers.Add(system);
                    container.Index = 0;
                }
            }
        }

        public void PlayParticle(string ID, Vector3 position)
        {
            PlayParticle(ID,position,Quaternion.identity);
        }

        public void PlayParticle(string ID, Vector3 position, Quaternion rotation)
        {
            foreach (var obj in Particles)
            {
                if (obj.Id == ID)
                {
                    if (obj.Index >= obj.ParticlesContainers.Count)
                    {
                        obj.Index = 0;
                    }

                    var container = obj.ParticlesContainers[obj.Index];
                    container.transform.position = position;
                    container.transform.rotation = rotation;
                    container.Play();
                    break;
                }
            }
        }
    }

    [Serializable]
    public class Particles
    {
        public string Id;
        public GameObject Example;
        [HideInInspector]public List<ParticleSystem> ParticlesContainers;
        public int Index;
    }
}