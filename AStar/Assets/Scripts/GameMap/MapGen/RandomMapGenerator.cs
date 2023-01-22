using System;
using System.Collections.Generic;
using System.Linq;
using Talespin.AStar.GameMap.MapTiles;
using UnityEngine;

namespace Talespin.AStar.GameMap.MapGen
{
    /// <summary>
    /// Generates Random Map.
    /// <para>
    /// Can potentially make 'impassable maps' by adding too many WaterTiles
    /// </para>
    /// </summary>
    public class RandomMapGenerator : MonoBehaviour, IMapGenerator
    {
        #region InnerTypes
        [System.Serializable]
        private struct TileSetting
        {
            public Tile TilePrefab;
            public float SpawnChance;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Settings related to Spawn-Chance of each TileType
        /// </summary>
        [SerializeField]
        [Tooltip("Settings related to Spawn-Chance of each TileType")]
        private List<TileSetting> tileSettings;
        /// <summary>
        /// Sum of SpawnChances
        /// </summary>
        private float totalChance;
        #endregion

        #region Methods
        /// <summary>
        /// Generates & Positions MapTiles
        /// </summary>
        /// <param name="parentTf">Parent-Transform for GameObjects</param>
        /// <param name="width">Map-Width (in Tiles)</param>
        /// <param name="height">Map-Height (in Tiles)</param>
        /// <returns>Generated Tiles for Map</returns>
        public Tile[,] GenerateMap(Transform parentTf, uint width, uint height)
        {
            if (width == 0)
                throw new ArgumentNullException(nameof(width), "Width cannot be null");
            if (height == 0)
                throw new ArgumentNullException(nameof(height), "Height cannot be null");
            Tile[,] result = new Tile[width, height];
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    result[x, y] = SpawnTile(parentTf, x, y);
            return result;
        }

        /// <summary>
        /// Self-Init for Generator
        /// </summary>
        private void Awake()
        {
            InitTileSettings();
        }

        /// <summary>
        /// Orders TileSettings to optimize finding random tile
        /// </summary>
        private void InitTileSettings()
        {
            totalChance = tileSettings.Sum(s => s.SpawnChance);
            tileSettings = tileSettings.OrderBy(s => s.SpawnChance).ToList(); // Order (smallest first)
            float currTotal = 0f;
            for (int i = 0; i < tileSettings.Count; i++)
            {
                TileSetting currSetting = tileSettings[i];
                float newSpawnChance = currSetting.SpawnChance * totalChance;
                currTotal += newSpawnChance;
                tileSettings[i] = new TileSetting { TilePrefab = currSetting.TilePrefab, SpawnChance = currTotal }; // Adjust to Total
            }
            totalChance = currTotal; // Fit (new) total so last tile is inside instead of outside of range
        }

        /// <summary>
        /// Spawns a (random) Tile at a Position in the Grid
        /// </summary>
        /// <param name="parentTf">Parent-Transform for GameObjects</param>
        /// <param name="width">Map-Position (in Tiles)</param>
        /// <param name="height">Map-Position (in Tiles)</param>
        /// <returns>Spawned Tile</returns>
        private Tile SpawnTile(Transform parentTf, int x, int y)
        {
            Tile spawnedTile = Instantiate(GetRandomTilePrefab(), parentTf);
            spawnedTile.SetPosition(x, y);
            return spawnedTile;
        }
        /// <summary>
        /// Gets Random TilePrefab based on RNG & TileSettings
        /// </summary>
        /// <returns>Random TilePrefab</returns>
        private Tile GetRandomTilePrefab()
        {
            float randomVal = UnityEngine.Random.Range(0, totalChance);
            for (int i = 0; i < tileSettings.Count; i++)
                if (tileSettings[i].SpawnChance >= randomVal)
                    return tileSettings[i].TilePrefab;
            return null;
        }
        #endregion
    }
}
