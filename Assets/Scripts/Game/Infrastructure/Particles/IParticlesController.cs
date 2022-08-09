using Game.Infrastructure.Services;
using UnityEngine;

namespace Game.Infrastructure.Particles
{
    public interface IParticlesController : IService
    {
        void PlayParticle(string ID, Vector3 position);
        void PlayParticle(string ID, Vector3 position, Quaternion rotation);
    }
}