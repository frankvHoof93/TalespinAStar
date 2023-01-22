using Talespin.AStar.GameMap;
using Talespin.AStar.GameMap.MapTiles;
using Talespin.AStar.Utils;
using UnityEngine;

namespace Talespin.AStar.UI
{
    /// <summary>
    /// Handles Positioning of Game-Camera
    /// </summary>
    [DefaultExecutionOrder(-1)] // Run Start before MapManager-Start
    [RequireComponent(typeof(Camera))]
    public class CameraManager : CachedBehaviour<CameraManager>
    {
        #region Constants
        /// <summary>
        /// Size of 1 Tile in WorldSpace
        /// </summary>
        private readonly Vector2 TILE_SIZE = new Vector2(1f, .75f);
        /// <summary>
        /// Range of Camera-Height for Grid-Size
        /// </summary>
        private readonly Vector2 HEIGHT_RANGE = new Vector2(5f, 85f);
        #endregion

        #region Properties
        /// <summary>
        /// Unity-Camera controlled by this Manager
        /// </summary>
        public Camera MainCam { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Self-Initialization
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            if (instance == this)
                MainCam = GetComponent<Camera>();
        }
        /// <summary>
        /// Hooks to Spawning of Grid-Event to Reposition after GridSpawn
        /// </summary>
        private void Start()
        {
            MapManager.Instance.OnSpawnMap += PositionCamForMap;
        }
        /// <summary>
        /// Positions Camera based on Grid-Size
        /// </summary>
        /// <param name="map">Grid that was Spawned</param>
        private void PositionCamForMap(Tile[,] map)
        {
            Vector2 mapSize = new Vector2Int(map.GetLength(0), map.GetLength(1));
            Vector2 middle = mapSize * .5f;
            float maxLength = Mathf.Max(mapSize.x, mapSize.y);
            float height = Map(maxLength, 1f, 125f, HEIGHT_RANGE.x, HEIGHT_RANGE.y);
            transform.position = new Vector3(middle.x * TILE_SIZE.x, height, middle.y * TILE_SIZE.y);
        }
        /// <summary>
        /// Maps Value to New Range
        /// </summary>
        private static float Map(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
        #endregion
    }
}
