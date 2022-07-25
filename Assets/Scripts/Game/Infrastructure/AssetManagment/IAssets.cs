using Game.Infrastructure.Services;
using UnityEngine;

namespace Game.Infrastructure.AssetManagment
{
    public interface IAssets : IService
    {
        GameObject Instantiate(string path);
        GameObject Instantiate(string path, Vector3 at);
    }
}