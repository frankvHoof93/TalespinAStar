using System;
using System.Collections.Generic;
using Talespin.AStar.Pathing;
using UnityEngine;
using UnityEngine.Assertions;

namespace Talespin.AStar.GameMap.MapTiles
{
    /// <summary>
    /// Represent a GameTile in the Map
    /// </summary>
    [RequireComponent(typeof(PathVisualizer))]
    public class Tile : MonoBehaviour, IAStarNode
    {
        #region Constants
        /// <summary>
        /// Directions to Neighbours in Array if Starting-Tile is on an Odd Row
        /// </summary>
        private static readonly Vector2Int[] NEIGHBOUR_OFFSETS_ODD_ROW = new Vector2Int[]
        {
            new Vector2Int(0, 1),
            new Vector2Int(1, 1),
            new Vector2Int(-1, 0),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(1, -1)
        };
        /// <summary>
        /// Directions to Neighbours in Array if Starting-Tile is on an Even Row
        /// </summary>
        private static readonly Vector2Int[] NEIGHBOUR_OFFSETS_EVEN_ROW = new Vector2Int[]
        {
            new Vector2Int(-1, 1),
            new Vector2Int(0, 1),
            new Vector2Int(-1, 0),
            new Vector2Int(1, 0),
            new Vector2Int(-1, -1),
            new Vector2Int(0, -1)
        };
        #endregion

        #region Properties
        /// <summary>
        /// Neighbours of Tile
        /// <para>
        /// Initialized upon first request (Lazy Init)
        /// </para>
        /// </summary>
        public IEnumerable<IAStarNode> Neighbours
        {
            get
            {
                if (neighbours == null || neighbours.Count == 0)
                    InitNeighbours();
                return neighbours;
            }
        }
        private List<IAStarNode> neighbours = null;

        /// <summary>
        /// Coordinates in Grid for this Tile
        /// </summary>
        public Vector2Int GridCoordinates { get; private set; }

        /// <summary>
        /// Size of Tile (offset for Placement)
        /// </summary>
        [SerializeField]
        [Tooltip("Size of Tile (offset for Placement)")]
        private Vector2 tileSize = new Vector2(1f, .75f);

        /// <summary>
        /// Cost to travel through this Tile
        /// </summary>
        [field: SerializeField]
        [field: Tooltip("Cost to travel through this Tile")]
        public uint TravelCost { get; private set; }

        /// <summary>
        /// Visualizer for Tile-Pathing
        /// </summary>
        private PathVisualizer visualizer;
        #endregion

        #region Methods
        #region Public
        /// <summary>
        /// The cost of traveling from the center of this Tile to the Center of the next tile is equal to half the cost of traveling through both.
        /// <para>
        /// Cost = TravelCost(this) * .5f + TravelCost(neighbour) * .5f
        /// </para>
        /// </summary>
        /// <param name="neighbour">Neighbour to Travel to</param>
        /// <returns></returns>
        public float CostTo(IAStarNode neighbour)
        {
            Tile neighbourTile = neighbour as Tile;
            if (neighbourTile == null)
                throw new InvalidOperationException("Cannot Calculate the Cost of traveling to a non-tile, as TravelCost is required and not a part of the IAStarNode-interface");
            return (TravelCost * .5f) + (neighbourTile.TravelCost * .5f);
        }

        /// <summary>
        /// Heuristic Function uses straight-line distance to calculate an estimate of cost.
        /// <para>
        /// Vector3.Distance(this, goal);
        /// </para>
        /// </summary>
        /// <param name="goal">Tile to estimate cost to</param>
        /// <returns>Straight-Line Distance from this Tile to Goal</returns>
        public float EstimatedCostTo(IAStarNode goal)
        {
            // We need to assume that 'goal' is at least a MonoBehaviour, so that we can find a Transform
            MonoBehaviour goalBehaviour = goal as MonoBehaviour;
            Assert.IsNotNull(goalBehaviour, "Goal must be of type MonoBehaviour");
            // Return Distance between Positions
            Transform goalTF = goalBehaviour.transform;
            return Vector3.Distance(transform.position, goalTF.position);
        }

        /// <summary>
        /// Set WorldSpace-Position based on Grid-Pos
        /// </summary>
        /// <param name="x">Column</param>
        /// <param name="y">Row</param>
        public void SetPosition(int x, int y)
        {
#if UNITY_EDITOR // Add GridPos to Name for Debug Purposes
            string name = gameObject.name;
            name = name.Remove(name.IndexOf("(Clone)"));
            gameObject.name = $"[{x}, {y}] - {name}";
#endif
            GridCoordinates = new Vector2Int(x, y);
            Vector3 newPos = new Vector3(x * tileSize.x, 0, y * tileSize.y);
            if (y % 2 != 0)
                newPos.x += 0.5f * tileSize.x;
            transform.position = newPos;
        }

        /// <summary>
        /// Visualize Tile as Part of Path
        /// </summary>
        /// <param name="indexInPath">Index of Tile in Path</param>
        /// <param name="pathLength">Total Length of Path</param>
        public void VisualizePathPiece(int indexInPath, int pathLength) => visualizer.SetPathTile(indexInPath, pathLength);
        /// <summary>
        /// Clear Visualization for Tile
        /// </summary>
        public void ClearPathVisualization() => visualizer.ClearPathTile();
        #endregion

        #region Unity
        /// <summary>
        /// Self-Initialization for Tile
        /// </summary>
        private void Awake()
        {
            visualizer = GetComponent<PathVisualizer>();
        }
        #endregion

        #region Private
        /// <summary>
        /// Grabs Neighbour-Tiles from Map
        /// </summary>
        private void InitNeighbours()
        {
            Tile[,] map = MapManager.Instance.Map;
            neighbours = new List<IAStarNode>();
            Vector2Int[] offsets = GridCoordinates.y % 2 == 0 ? NEIGHBOUR_OFFSETS_EVEN_ROW : NEIGHBOUR_OFFSETS_ODD_ROW;
            for (int i = 0; i < offsets.Length; i++)
            {
                Vector2Int neighbourPos = GridCoordinates + offsets[i];
                try
                {
                    Tile neighbour = map[neighbourPos.x, neighbourPos.y];
                    neighbours.Add(neighbour);
                }
                catch (IndexOutOfRangeException) { } // Do Nothing (Outside of Grid)
            }
        }
        #endregion
        #endregion
    }
}
