using UnityEngine;

namespace Talespin.AStar.GameMap.MapTiles
{
    [RequireComponent(typeof(Renderer))]
    public class PathVisualizer : MonoBehaviour
    {
        private Renderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        /// <summary>
        /// Visualize Tile as Part of Path
        /// </summary>
        /// <param name="isStart">Is Start Tile for Path?</param>
        /// <param name="isEnd">Is End Tile for Path?</param>
        public void SetPathTile(bool isStartTile, bool isEndTile)
        {
            Debug.Log($"Visualize PathPiece: {gameObject.name} - {isStartTile} - {isEndTile}", gameObject);
            Vector3 localPos = transform.localPosition;
            localPos.y = .25f;
            transform.localPosition = localPos;
            _renderer.material.SetColor("_Color", Color.red);
        }
        /// <summary>
        /// Clear Visualization for Tile
        /// </summary>
        public void ClearPathTile()
        {
            Debug.Log($"Clear PathPiece: {gameObject.name}", gameObject);
            Vector3 localPos = transform.localPosition;
            localPos.y = 0f;
            transform.localPosition = localPos;
            _renderer.material.SetColor("_Color", Color.white);
        }
    }
}
