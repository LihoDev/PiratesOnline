using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace PiratesOnline.Domain.Service
{
    public class AddressableAssetProvider : IAssetProvider
    {
        // Cache of downloaded assets to prevent duplication
        private readonly Dictionary<string, AsyncOperationHandle> _cache = new Dictionary<string, AsyncOperationHandle>();

        public async Task<T> LoadAssetAsync<T>(string address) where T : Object
        {
            if (_cache.TryGetValue(address, out var cachedHandle))
            {
                return cachedHandle.Result as T;
            }

            var handle = Addressables.LoadAssetAsync<T>(address);
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _cache[address] = handle;
                return handle.Result;
            }

            Debug.LogError($"[AssetProvider] Error loading asset by key: {address}");
            Addressables.Release(handle);
            return null;
        }

        public async Task<GameObject> InstantiateAsync(string address, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var prefab = await LoadAssetAsync<GameObject>(address);
            if (prefab != null)
            {
                return Object.Instantiate(prefab, position, rotation, parent);
            }
            return null;
        }

        public void ReleaseAsset(string address)
        {
            if (_cache.TryGetValue(address, out var handle))
            {
                Addressables.Release(handle);
                _cache.Remove(address);
            }
        }

        public void Cleanup()
        {
            foreach (var handle in _cache.Values)
            {
                Addressables.Release(handle);
            }
            _cache.Clear();
            Debug.Log("[AssetProvider] Cache Addressables cleared.");
        }
    }
}