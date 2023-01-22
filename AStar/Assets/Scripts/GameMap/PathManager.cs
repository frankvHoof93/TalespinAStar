using System.Collections;
using System.Collections.Generic;
using Talespin.AStar.Pathing;
using Talespin.AStar.PlayerInput;
using UnityEngine;

namespace Talespin.AStar.GameMap
{
    public class PathManager : MonoBehaviour
    {
        private IList<IAStarNode> currPath;

        private void Start()
        {
            MouseInputManager.Instance.OnSelectionChanged += HandleSelectionChanged;
        }

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
                        (pathNode as Tile).VisualizePathPiece(i == 0, i == currPath.Count - 1);
                }
            }
        }

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