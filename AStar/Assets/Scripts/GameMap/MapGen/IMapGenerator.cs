using UnityEngine;

namespace Talespin.AStar.GameMap.MapGen
{
    /// <summary>
    /// Interface for MapGeneration-Algorithm(s)
    /// </summary>
    public interface IMapGenerator
    {
        Tile[,] GenerateMap(Transform parentTf, uint width, uint height);
    }
}
