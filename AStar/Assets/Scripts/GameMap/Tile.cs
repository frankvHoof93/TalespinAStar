using System.Collections.Generic;
using System.Linq;
using Talespin.AStar.Pathing;
using UnityEngine;
using UnityEngine.Assertions;

namespace Talespin.AStar.GameMap
{
    /// <summary>
    /// Represent a GameTile in the Map
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public class Tile : MonoBehaviour, IAStarNode
    {
        #region Properties
        /// <summary>
        /// Neighbours of Tile
        /// <para>
        /// Initialized upon first request
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
        public int TravelCost { get; private set; }

        /// <summary>
        /// Renderer for Tile (used for Visualization)
        /// </summary>
        private Renderer _renderer;
        #endregion

        #region Methods
        /// <summary>
        /// The cost of traveling from the Tile BEFORE entering this one, to a Neighour (i.e. the Tile AFTER this one)
        /// is equal to the TravelCost through THIS tile.
        /// </summary>
        /// <param name="neighbour">Neighbour to Travel to</param>
        /// <returns></returns>
        public float CostTo(IAStarNode neighbour)
        {
            return TravelCost;
        }

        /// <summary>
        /// Heuristic Function uses straight-line distance to calculate an estimate of cost.
        /// <para>
        /// Vector3.Distance(this, goal);
        /// </para>
        /// </summary>
        /// <param name="goal"></param>
        /// <returns></returns>
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
            Vector3 newPos = new Vector3(x * tileSize.x, 0, y * tileSize.y);
            if (y % 2 != 0)
                newPos.x += 0.5f * tileSize.x;
            transform.position = newPos;
        }


        public void VisualizePathPiece(bool isStart, bool isEnd)
        {
            Debug.Log($"Visualize PathPiece: {gameObject.name} - {isStart} - {isEnd}", this.gameObject);
            _renderer.material.SetColor("_Color", Color.red);
        }

        public void ClearPathVisualization()
        {
            Debug.Log($"Clear PathPiece: {gameObject.name}", this.gameObject);
            _renderer.material.SetColor("_Color", Color.white);
        }

        /// <summary>
        /// Self-Initialization for Tile
        /// </summary>
        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        /// <summary>
        /// Quick & dirty init through Physics-Engine
        /// TODO: Should use Math instead
        /// </summary>
        private void InitNeighbours()
        {
            neighbours = Physics.OverlapSphere(transform.position, tileSize.x * .75f, 1 << gameObject.layer)
                .Select(c => c.GetComponent<IAStarNode>()).ToList();
        }
        #endregion
    }
}
