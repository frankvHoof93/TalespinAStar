using System;
using Talespin.AStar.GameMap.MapGen;
using Talespin.AStar.GameMap.MapTiles;
using Talespin.AStar.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace Talespin.AStar.GameMap
{
    /// <summary>
    /// Maintains Tiles in Map
    /// </summary>
    [RequireComponent(typeof(IMapGenerator))]
    public class MapManager : CachedBehaviour<MapManager>
    {
        #region Properties
        /// <summary>
        /// Event Fired when (new) Map is Spawned
        /// </summary>
        public event Action<Tile[,]> OnSpawnMap;

        /// <summary>
        /// Map-Tiles
        /// </summary>
        public Tile[,] Map { get; private set; }
        /// <summary>
        /// Size for (default) Spawned Map (in Tiles)
        /// </summary>
        [field: SerializeField]
        [field: Tooltip("Size for (default) Spawned Map (in Tiles)")]
        public Vector2Int MapSize { get; private set; }
        /// <summary>
        /// MapGenerator-Algorithm used to Generate Map
        /// </summary>
        private IMapGenerator mapGenerator;
        #endregion

        #region Methods
        /// <summary>
        /// Spawns a new Map using <see cref="MapSize"/> for Size
        /// </summary>
        public void SpawnMap()
        {
            SpawnMap(null, null);
        }

        /// <summary>
        /// Spawns a new Map
        /// </summary>
        /// <param name="width">Width for Map in Tiles. Set NULL to use <see cref="MapSize"/></param>
        /// <param name="height">Height for Map in Tiles. Set NULL to use <see cref="MapSize"/></param>
        public void SpawnMap(uint? width, uint? height)
        {
            if (Map != null)
                DestroyMap();
            if (!width.HasValue)
                width = (uint)MapSize.x;
            if (!height.HasValue)
                height = (uint)MapSize.y;
            if (width > 125)
            {
                Debug.LogWarning("Clipped Width to 125 to prevent extremely long duration for MapGen");
                width = 125;
            }
            if (height > 125)
            {
                Debug.LogWarning("Clipped Height to 125 to prevent extremely long duration for MapGen");
                height = 125;
            }
            Map = mapGenerator.GenerateMap(transform, width.Value, height.Value);
            OnSpawnMap?.Invoke(Map);
        }
        /// <summary>
        /// Destroys active Map
        /// </summary>
        public void DestroyMap()
        {
            if (Map == null)
                return;
            for (int x = 0; x < Map.GetLength(0); x++)
                for (int y = 0; y < Map.GetLength(1); y++)
                    Destroy(Map[x, y].gameObject);
            Map = null;
        }
        /// <summary>
        /// Self-Initialization
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            if (instance == this)
            {
                mapGenerator = GetComponent<IMapGenerator>();
                Assert.IsNotNull(mapGenerator, "MapManager requires a MapGenerator-Algorithm");
            }
        }
        /// <summary>
        /// Spawns Map on Start of Game
        /// </summary>
        private void Start()
        {
            SpawnMap();
        }
        /// <summary>
        /// Checks if <see cref="MapSize"/> is Valid
        /// </summary>
        private void OnValidate()
        {
            if (MapSize.x <= 0 || MapSize.y <= 0)
                Debug.LogError("Invalid MapSize", this);
        }
        #endregion
    }
}
