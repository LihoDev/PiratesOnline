using Mirror;
using PiratesOnline.Domain.Service;
using PiratesOnline.Presentation.Player;
using UnityEngine;
using Zenject;

namespace PiratesOnline.Infrastructure.Network
{
    public class PiratesNetworkManager : NetworkManager
    {
        private IServerDataService _dataService;
        private IMapService _mapService;
        private IInstantiator _instantiator;

        [Inject]
        public void Construct(IServerDataService dataService, IMapService mapService, IInstantiator instantiator)
        {
            _dataService = dataService;
            _mapService = mapService;
            _instantiator = instantiator;
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            string accountId = $"Player_{conn.connectionId}";
            var playerData = _dataService.GetPlayerData(accountId);
            GameObject playerInstance = _instantiator.InstantiatePrefab(playerPrefab);

            if (playerData.LastPosition == Vector2.zero)
            {
                playerData.LastPosition = _mapService.GetRandomEdgeSpawnPosition();
            }
            playerInstance.transform.position = playerData.LastPosition;

            var shipController = playerInstance.GetComponent<PlayerShipController>();
            shipController.InitServerData(playerData.Stats);

            NetworkServer.AddPlayerForConnection(conn, playerInstance);
            Debug.Log("added player");
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            base.OnServerDisconnect(conn);
            // Player data save
        }
    }
}