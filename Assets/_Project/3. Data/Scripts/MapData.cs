using UnityEngine;

namespace PiratesOnline.Domain.Data
{
    public enum BiomeType
    {
        Normal,
        Swamp, 
        Windy,
        Storm 
    }

    public struct MapCell
    {
        public Vector2Int GridPosition;
        public BiomeType Biome;
        public bool HasPort;
    }

    [System.Serializable]
    public class BiomeConfig
    {
        public BiomeType Type;
        public float SpeedMultiplier;
        public Color DebugColor;
    }
}