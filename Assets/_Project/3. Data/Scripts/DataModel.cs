using UnityEngine;

namespace PiratesOnline.Domain.Data
{
    [System.Serializable]
    public struct ShipStats
    {
        public int MaxHp;
        public int CurrentHp;
        public float MaxBuoyancy;
        public float CurrentBuoyancy;
        public float Speed;
        public int MastsCount;
        public int DecksCount;
        public string SkinAddress; // Key for Addressables
        public Color ShipColor;
    }

    [System.Serializable]
    public struct ItemData
    {
        public string ItemId; // Key for Addressables for item data
        public int SlotIndex;
        public bool IsEquipped;
    }

    public struct PlayerSaveData
    {
        public string AccountId;
        public int Gold;
        public ShipStats Stats;
        public ItemData[] Inventory;
        public Vector2 LastPosition; // Position on a map after disconect
    }
}