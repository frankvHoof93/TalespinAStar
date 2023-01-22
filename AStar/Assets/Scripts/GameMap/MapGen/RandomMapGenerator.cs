using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Talespin.AStar.GameMap.MapGen
{
    /// <summary>
    /// Generates Random Map.
    /// <para>
    /// Can potentially make 'impassable maps' by adding too much WaterTiles
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

            internal void SetSpawnChance(float newChance)
            {
                SpawnChance = newChance;
            }
        }
        #endregion

        [SerializeField]
        private List<TileSetting> tileSettings;

        private float totalChance;

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
                tileSettings[i] = new TileSetting {TilePrefab = currSetting.TilePrefab, SpawnChance = currTotal }; // Adjust to Total
            }
            totalChance = currTotal; // Fit (new) total so last tile is inside instead of outside of range
        }

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

        private Tile SpawnTile(Transform parentTf, int x, int y)
        {
            Tile spawnedTile = Instantiate(GetRandomTilePrefab(), parentTf);
            spawnedTile.SetPosition(x, y);
            return spawnedTile;
        }

        private Tile GetRandomTilePrefab()
        {
            Debug.Log("Total " + totalChance);
            float randomVal = UnityEngine.Random.Range(0, totalChance);
            Debug.Log("RandomVal " + randomVal);
            for (int i = 0; i < tileSettings.Count; i++)
            {
                Debug.Log(tileSettings[i].SpawnChance);
                if (tileSettings[i].SpawnChance >= randomVal)
                    return tileSettings[i].TilePrefab;
            }
            return null;
        }
    }
}
