using Mirror;
using PiratesOnline.Domain.Service;
using UnityEngine;
using Zenject;

namespace PiratesOnline.Infrastructure.Network
{
    public class PiratesNetworkManager : NetworkManager
    {
        private IServerDataService _dataService;

        [Inject]
        public void Construct(IServerDataService dataService)
        {
            _dataService = dataService;
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            string accountId = $"Player_{conn.connectionId}";
            var playerData = _dataService.GetPlayerData(accountId);
            GameObject playerInstance = Instantiate(playerPrefab);

            // var controller = playerInstance.GetComponent<PlayerNetworkController>();
            // controller.InitData(playerData);

            NetworkServer.AddPlayerForConnection(conn, playerInstance);

            playerInstance.transform.position = playerData.LastPosition; //!!! For old players only, no random 
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            base.OnServerDisconnect(conn);
            // Player data save
        }
    }
}