using Talespin.AStar.GameMap.MapGen;
using UnityEngine;
using UnityEngine.Assertions;

namespace Talespin.AStar.GameMap
{
    [RequireComponent(typeof(IMapGenerator))]
    public class MapManager : MonoBehaviour
    {
        public Tile[,] Map { get; private set; }

        [SerializeField]
        private Vector2Int mapSize;

        private IMapGenerator mapGenerator;

        private void Awake()
        {
            mapGenerator = GetComponent<IMapGenerator>();
            Assert.IsNotNull(mapGenerator, "MapManager requires a MapGenerator-Algorithm");
        }

        private void Start()
        {
            Map = mapGenerator.GenerateMap(transform, (uint)mapSize.x, (uint)mapSize.y);
        }

        private void OnValidate()
        {
            if (mapSize.x <= 0 || mapSize.y <= 0)
                Debug.LogError("Invalid MapSize", this);
        }

    }
}
