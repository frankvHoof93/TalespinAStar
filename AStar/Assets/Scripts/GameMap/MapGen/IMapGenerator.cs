using UnityEngine;

namespace Talespin.AStar.GameMap.MapGen
{
    /// <summary>
    /// Interface for MapGeneration-Algorithm(s)
    /// </summary>
    public interface IMapGenerator
    {
        /// <summary>
        /// Generates & Positions MapTiles
        /// </summary>
        /// <param name="parentTf">Parent-Transform for GameObjects</param>
        /// <param name="width">Map-Width (in Tiles)</param>
        /// <param name="height">Map-Height (in Tiles)</param>
        /// <returns>Generated Tiles for Map</returns>
        Tile[,] GenerateMap(Transform parentTf, uint width, uint height);
    }
}
