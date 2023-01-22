using UnityEngine;

namespace Talespin.AStar.GameMap.MapTiles
{
    /// <summary>
    /// Visualizes Tile based on Location in Path
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public class PathVisualizer : MonoBehaviour
    {
        #region Properties
        /// <summary>
        /// Color to set if Tile is Start-Position for Path
        /// </summary>
        [SerializeField]
        [Tooltip("Color to set if Tile is Start-Position for Path")]
        private Color pathStartColor = Color.blue;
        /// <summary>
        /// Color to set if Tile is Middle-Position for Path
        /// </summary>
        [SerializeField]
        [Tooltip("Color to set if Tile is Middle-Position for Path")]
        private Color pathPieceColor = Color.red;
        /// <summary>
        /// Color to set if Tile is End-Position for Path
        /// </summary>
        [SerializeField]
        [Tooltip("Color to set if Tile is End-Position for Path")]
        private Color pathEndColor = Color.green;

        /// <summary>
        /// Renderer for Tile
        /// </summary>
        private Renderer _renderer;
        #endregion

        #region Methods
        /// <summary>
        /// Visualize Tile as Part of Path
        /// </summary>
        /// <param name="isStart">Is Start Tile for Path?</param>
        /// <param name="isEnd">Is End Tile for Path?</param>
        public void SetPathTile(bool isStartTile, bool isEndTile)
        {
            Vector3 localPos = transform.localPosition;
            localPos.y = .25f;
            transform.localPosition = localPos;
            _renderer.material.SetColor("_Color", isStartTile ? pathStartColor : isEndTile ? pathEndColor : pathPieceColor);
        }
        /// <summary>
        /// Clear Visualization for Tile
        /// </summary>
        public void ClearPathTile()
        {
            Vector3 localPos = transform.localPosition;
            localPos.y = 0f;
            transform.localPosition = localPos;
            _renderer.material.SetColor("_Color", Color.white);
        }

        /// <summary>
        /// Self-Initialization for PathVisualizer
        /// </summary>
        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }
        #endregion
    }
}
