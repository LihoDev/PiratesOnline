using Mirror;
using PiratesOnline.Domain.Service;
using UnityEngine;
using Zenject;

namespace PiratesOnline.Infrastructure.Network
{
    public class MapNetworkSynchronizer : NetworkBehaviour
    {
        private IMapService _mapService;

        [SyncVar(hook = nameof(OnSeedChanged))]
        private int _mapSeed;

        [Inject]
        public void Construct(IMapService mapService)
        {
            _mapService = mapService;
        }

        public override void OnStartServer()
        {
            // The server (Host) comes up with the generation seed
            _mapSeed = Random.Range(1000, 99999);

            // The server generates a map
            _mapService.GenerateMap(_mapSeed);
        }

        private void OnSeedChanged(int oldSeed, int newSeed)
        {
            // Client receive grain from server and generate the same map
            if (isClientOnly)
            {
                _mapService.GenerateMap(newSeed);
            }
        }
    }
}