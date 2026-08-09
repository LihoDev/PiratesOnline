using Mirror;
using PiratesOnline.Domain.Service;
using UnityEngine;
using Zenject;

namespace PiratesOnline.Infrastructure.Network
{
    public class MapNetworkSynchronizer : NetworkBehaviour
    {
        private IMapService _mapService;

        // SyncVar автоматически передается всем клиентам при изменении на сервере.
        // Когда клиент получает новое значение, вызывается hook OnSeedChanged.
        [SyncVar(hook = nameof(OnSeedChanged))]
        private int _mapSeed;

        [Inject]
        public void Construct(IMapService mapService)
        {
            _mapService = mapService;
        }

        public override void OnStartServer()
        {
            // Сервер (Хост) придумывает зерно генерации
            _mapSeed = Random.Range(1000, 99999);

            // Сервер сам тоже генерирует карту
            _mapService.GenerateMap(_mapSeed);
        }

        private void OnSeedChanged(int oldSeed, int newSeed)
        {
            // Клиенты получают зерно от сервера и генерируют такую же карту
            if (isClientOnly)
            {
                _mapService.GenerateMap(newSeed);
            }
        }
    }
}