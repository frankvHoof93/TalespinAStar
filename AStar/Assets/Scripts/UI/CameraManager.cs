using Talespin.AStar.GameMap;
using Talespin.AStar.GameMap.MapTiles;
using Talespin.AStar.Utils;
using UnityEngine;

namespace Talespin.AStar.UI
{
    [DefaultExecutionOrder(-1)] // Run Start before MapManager-Start
    [RequireComponent(typeof(Camera))]
    public class CameraManager : CachedBehaviour<CameraManager>
    {
        private readonly Vector2 TILE_SIZE = new Vector2(1f, .75f);

        private readonly Vector2 HEIGHT_RANGE = new Vector2(5f, 85f);

        public Camera MainCam { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            if (instance == this)
                MainCam = GetComponent<Camera>();
        }

        private void Start()
        {
            MapManager.Instance.OnSpawnMap += PositionCamForMap;
        }

        private void PositionCamForMap(Tile[,] map)
        {
            Vector2 mapSize = new Vector2Int(map.GetLength(0), map.GetLength(1));
            Vector2 middle = mapSize * .5f;
            float maxLength = Mathf.Max(mapSize.x, mapSize.y);
            float height = Map(maxLength, 1f, 125f, HEIGHT_RANGE.x, HEIGHT_RANGE.y);
            transform.position = new Vector3(middle.x * TILE_SIZE.x, height, middle.y * TILE_SIZE.y);
        }

        public static float Map(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
    }
}
