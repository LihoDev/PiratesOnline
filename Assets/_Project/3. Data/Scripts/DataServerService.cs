using PiratesOnline.Domain.Data;
using System.Collections.Generic;
using UnityEngine;

namespace PiratesOnline.Domain.Service
{
    public interface IServerDataService
    {
        PlayerSaveData GetPlayerData(string accountId);
        void SavePlayerData(string accountId, PlayerSaveData data);
    }

    public class MockServerDataService : IServerDataService
    {
        // Simulating a database in host memory
        private Dictionary<string, PlayerSaveData> _database = new Dictionary<string, PlayerSaveData>();

        public PlayerSaveData GetPlayerData(string accountId)
        {
            if (_database.TryGetValue(accountId, out var data))
            {
                return data;
            }

            // Initializing a new player if it is not in the DB
            var newData = CreateDefaultPlayer(accountId);
            _database[accountId] = newData;
            return newData;
        }

        public void SavePlayerData(string accountId, PlayerSaveData data)
        {
            _database[accountId] = data;
            Debug.Log($"[Server] Player data {accountId} saved.");
        }

        private PlayerSaveData CreateDefaultPlayer(string accountId)
        {
            return new PlayerSaveData
            {
                AccountId = accountId,
                Gold = 1000,
                Stats = new ShipStats
                {
                    MaxHp = 100,
                    CurrentHp = 100,
                    MaxBuoyancy = 100f,
                    CurrentBuoyancy = 100f,
                    Speed = 5f,
                    MastsCount = 1,
                    DecksCount = 1,
                    SkinAddress = "Skin_DefaultSloop",
                    ShipColor = Random.ColorHSV()
                },
                Inventory = new ItemData[0],
                LastPosition = Vector2.zero
            };
        }
    }
}