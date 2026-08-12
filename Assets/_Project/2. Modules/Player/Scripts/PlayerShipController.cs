using Mirror;
using PiratesOnline.Domain.Data;
using PiratesOnline.Domain.Service;
using UnityEngine;
using Zenject;

namespace PiratesOnline.Presentation.Player
{
    public class PlayerShipController : NetworkBehaviour
    {
        [SyncVar(hook = nameof(OnStatsChanged))]
        private ShipStats _stats;

        public ShipStats Stats => _stats;

        private IAssetProvider _assetProvider;
        private GameObject _shipVisuals;

        [Inject]
        public void Construct(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        private void Awake()
        {
            ProjectContext.Instance.Container.Inject(this);
        }

        [Server]
        public void InitServerData(ShipStats stats)
        {
            _stats = stats;
        }

        private async void OnStatsChanged(ShipStats oldStats, ShipStats newStats)
        {
            // If the skin has changed, unload the old one
            if (_shipVisuals != null && oldStats.SkinAddress != newStats.SkinAddress)
            {
                _assetProvider.ReleaseAsset(oldStats.SkinAddress);
                Destroy(_shipVisuals);
                _shipVisuals = null;
            }

            // Загружаем новый скин через Addressables, если он еще не загружен
            if (_shipVisuals == null && !string.IsNullOrEmpty(newStats.SkinAddress))
            {
                _shipVisuals = await _assetProvider.InstantiateAsync(
                    newStats.SkinAddress,
                    transform.position,
                    transform.rotation,
                    transform // Делаем визуал дочерним объектом
                );
            }

            // Применяем цвет
            if (_shipVisuals != null)
            {
                var spriteRenderer = _shipVisuals.GetComponentInChildren<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = newStats.ShipColor;
                }
            }
        }

        private void OnDestroy()
        {
            if (!string.IsNullOrEmpty(_stats.SkinAddress) && _assetProvider != null)
            {
                _assetProvider.ReleaseAsset(_stats.SkinAddress);
            }
        }
    }
}