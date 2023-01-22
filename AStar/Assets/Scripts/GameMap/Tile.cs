using System.Collections.Generic;
using Talespin.AStar.Pathing;
using UnityEngine;
using UnityEngine.Assertions;

namespace Talespin.AStar.GameMap
{
    [RequireComponent(typeof(Renderer))]
    public class Tile : MonoBehaviour, IAStarNode
    {
        /// <summary>
        /// Cost to travel through this Tile
        /// </summary>
        [field: SerializeField]
        [field: Tooltip("Cost to travel through this Tile")]
        public int TravelCost { get; private set; }

        private Renderer _renderer;

        public IEnumerable<IAStarNode> Neighbours => neighbours;
        private readonly List<IAStarNode> neighbours = new List<IAStarNode>();

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

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }
    }
}
