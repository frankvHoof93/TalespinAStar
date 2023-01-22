using Talespin.AStar.GameMap.MapGen;
using UnityEngine;
using UnityEngine.Assertions;

namespace Talespin.AStar.GameMap
{
    /// <summary>
    /// Maintains Tiles in Map
    /// </summary>
    [RequireComponent(typeof(IMapGenerator))]
    public class MapManager : MonoBehaviour
    {
        #region Properties
        /// <summary>
        /// Map-Tiles
        /// </summary>
        public Tile[,] Map { get; private set; }
        /// <summary>
        /// Size for (default) Spawned Map (in Tiles)
        /// </summary>
        [SerializeField]
        [Tooltip("Size for (default) Spawned Map (in Tiles)")]
        private Vector2Int mapSize;
        /// <summary>
        /// MapGenerator-Algorithm used to Generate Map
        /// </summary>
        private IMapGenerator mapGenerator;
        #endregion

        #region Methods
        /// <summary>
        /// Spawns a new Map
        /// </summary>
        /// <param name="width">Width for Map in Tiles. Set NULL to use <see cref="mapSize"/></param>
        /// <param name="height">Height for Map in Tiles. Set NULL to use <see cref="mapSize"/></param>
        public void SpawnMap(uint? width = null, uint? height = null)
        {
            if (Map != null)
                DestroyMap();
            if (!width.HasValue)
                width = (uint)mapSize.x;
            if (!height.HasValue)
                height = (uint)mapSize.y;
            Map = mapGenerator.GenerateMap(transform, width.Value, height.Value);
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
        private void Awake()
        {
            mapGenerator = GetComponent<IMapGenerator>();
            Assert.IsNotNull(mapGenerator, "MapManager requires a MapGenerator-Algorithm");
        }
        /// <summary>
        /// Spawns Map on Start of Game
        /// </summary>
        private void Start()
        {
            SpawnMap();
        }
        /// <summary>
        /// Checks if <see cref="mapSize"/> is Valid
        /// </summary>
        private void OnValidate()
        {
            if (mapSize.x <= 0 || mapSize.y <= 0)
                Debug.LogError("Invalid MapSize", this);
        }
        #endregion
    }
}
