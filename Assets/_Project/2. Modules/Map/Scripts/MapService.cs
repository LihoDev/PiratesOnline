using PiratesOnline.Domain.Data;
using System.Collections.Generic;
using UnityEngine;

namespace PiratesOnline.Domain.Service
{
    public interface IMapService
    {
        int Width { get; }
        int Height { get; }
        float CellSize { get; }

        void GenerateMap(int seed);

        MapCell GetCellAt(Vector2 worldPosition);
        
        float GetSpeedMultiplier(Vector2 worldPosition);
        
        Vector3 GetRandomEdgeSpawnPosition();

        MapCell[,] GetGrid();
    }

    public class MapService : IMapService
    {
        public int Width { get; } = 5;
        public int Height { get; } = 5;
        public float CellSize { get; } = 10f;

        private MapCell[,] _grid;
        private System.Random _rng;

        private readonly Dictionary<BiomeType, float> _biomeSpeedModifiers = new Dictionary<BiomeType, float>
        {
            { BiomeType.Normal, 1.0f },
            { BiomeType.Swamp, 0.5f }, 
            { BiomeType.Windy, 1.5f },
            { BiomeType.Storm, 1.0f }
        };

        public void GenerateMap(int seed)
        {
            _grid = new MapCell[Width, Height];
            _rng = new System.Random(seed);

            float offsetX = _rng.Next(-10000, 10000);
            float offsetY = _rng.Next(-10000, 10000);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    float noise = Mathf.PerlinNoise((x + offsetX) * 0.1f, (y + offsetY) * 0.1f);
                    BiomeType biome = DetermineBiome(noise);
                    bool hasPort = false;
                    if (x > 2 && x < Width - 3 && y > 2 && y < Height - 3)
                    {
                        hasPort = _rng.NextDouble() < 0.02;
                    }

                    _grid[x, y] = new MapCell
                    {
                        GridPosition = new Vector2Int(x, y),
                        Biome = biome,
                        HasPort = hasPort
                    };
                }
            }
            Debug.Log($"[MapService] Map {Width}x{Height} generated. Seed: {seed}");
        }

        private BiomeType DetermineBiome(float noise)
        {
            if (noise < 0.3f) return BiomeType.Swamp;
            if (noise > 0.7f) return BiomeType.Windy;
            if (noise > 0.6f && noise <= 0.7f) return BiomeType.Storm;
            return BiomeType.Normal;
        }

        public MapCell GetCellAt(Vector2 worldPosition)
        {
            if (_grid == null) return default;

            int x = Mathf.FloorToInt(worldPosition.x / CellSize);
            int y = Mathf.FloorToInt(worldPosition.y / CellSize);

            x = Mathf.Clamp(x, 0, Width - 1);
            y = Mathf.Clamp(y, 0, Height - 1);

            return _grid[x, y];
        }

        public float GetSpeedMultiplier(Vector2 worldPosition)
        {
            var cell = GetCellAt(worldPosition);
            return _biomeSpeedModifiers.TryGetValue(cell.Biome, out float speed) ? speed : 1.0f;
        }

        public Vector3 GetRandomEdgeSpawnPosition()
        {
            // Players spawn at the edges of the map
            bool isVerticalEdge = _rng.Next(0, 2) == 0;
            int x = isVerticalEdge ? (_rng.Next(0, 2) == 0 ? 0 : Width - 1) : _rng.Next(0, Width);
            int y = isVerticalEdge ? _rng.Next(0, Height) : (_rng.Next(0, 2) == 0 ? 0 : Height - 1);

            // Return world coordinates (cell center)
            return new Vector3(x * CellSize + (CellSize / 2), y * CellSize + (CellSize / 2), 0);
        }

        public MapCell[,] GetGrid() => _grid;
    }
}