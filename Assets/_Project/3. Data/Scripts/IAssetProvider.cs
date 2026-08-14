using System.Threading.Tasks;
using UnityEngine;

namespace PiratesOnline.Domain.Service
{
    public interface IAssetProvider
    {
        // Asynchronous asset loading
        Task<T> LoadAssetAsync<T>(string address) where T : Object;
        // Loading a prefab and instantiating it
        Task<GameObject> InstantiateAsync(string address, Vector3 position, Quaternion rotation, Transform parent = null);
        // Freeing a resource from memory
        void ReleaseAsset(string address);
        // Clearing the entire cache (for example, when changing a scene)
        void Cleanup();
    }
}