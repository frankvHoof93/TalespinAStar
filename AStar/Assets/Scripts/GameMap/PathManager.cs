using System.Collections.Generic;
using Talespin.AStar.GameMap.MapTiles;
using Talespin.AStar.Pathing;
using Talespin.AStar.PlayerInput;
using UnityEngine;

namespace Talespin.AStar.GameMap
{
    /// <summary>
    /// Handles Path-Generation on Map
    /// </summary>
    public class PathManager : MonoBehaviour
    {
        /// <summary>
        /// Current Path
        /// </summary>
        private IList<IAStarNode> currPath;

        /// <summary>
        /// Hooks to InputEvents
        /// </summary>
        private void Start()
        {
            MouseInputManager mgr = MouseInputManager.Instance;
            mgr.OnStartTileChanged += (prev, next) => HandleTileChanged(true, prev, next);
            mgr.OnEndTileChanged += (prev, next) => HandleTileChanged(false, prev, next);
            mgr.OnSelectionChanged += HandleSelectionChanged;
        }

        /// <summary>
        /// Sets Visualization for Start- or End-Tile when Selected
        /// </summary>
        /// <param name="isPathStart">Whether this is the Start-Tile (or the End-Tile)</param>
        /// <param name="prev">Previously Selected Tile</param>
        /// <param name="next">Newly Selected Tile</param>
        private void HandleTileChanged(bool isPathStart, Tile prev, Tile next)
        {
            if (prev != null)
                prev.ClearPathVisualization();
            if (next != null)
                next.VisualizePathPiece(isPathStart ? 0 : -2, -1);
        }

        /// <summary>
        /// Checks if both Start- and End-Tile have been selected.
        /// If both have been slected, generates a new Path
        /// </summary>
        private void HandleSelectionChanged()
        {
            MouseInputManager input = MouseInputManager.Instance;
            if (input == null)
                return;
            if (input.StartTile != null && input.EndTile != null)
            {
                if (currPath != null && currPath.Count > 0)
                    ClearPath(); // Clear Existing Path
                currPath = Pathing.AStar.GetPath(input.StartTile, input.EndTile);
                for (int i = 0; i < currPath.Count; i++)
                {
                    IAStarNode pathNode = currPath[i];
                    if (pathNode as Tile != null)
                        (pathNode as Tile).VisualizePathPiece(i, currPath.Count);
                }
            }
        }

        /// <summary>
        /// Clears (visualization for) Current Path
        /// </summary>
        private void ClearPath()
        {
            for (int i = 0; i < currPath.Count; i++)
            {
                IAStarNode pathNode = currPath[i];
                if (pathNode as Tile != null)
                    (pathNode as Tile).ClearPathVisualization();
            }
            currPath.Clear();
        }
    }
}
