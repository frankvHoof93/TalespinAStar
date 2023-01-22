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
        /// <summary>
        /// MaterialPropertyBlock used to update Renderer-Properties
        /// </summary>
        private MaterialPropertyBlock mpBlock;
        /// <summary>
        /// Property-ID for Color-Property in Shader
        /// </summary>
        private static int? colorPropID;
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
            SetColor(isStartTile ? pathStartColor : isEndTile ? pathEndColor : pathPieceColor);
        }
        /// <summary>
        /// Clear Visualization for Tile
        /// </summary>
        public void ClearPathTile()
        {
            Vector3 localPos = transform.localPosition;
            localPos.y = 0f;
            transform.localPosition = localPos;
            SetColor(Color.white);
        }

        /// <summary>
        /// Self-Initialization for PathVisualizer
        /// </summary>
        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            mpBlock = new MaterialPropertyBlock();
        }
        /// <summary>
        /// Sets Color to material
        /// </summary>
        /// <param name="c">Color to set</param>
        private void SetColor(Color c)
        {
            if (!colorPropID.HasValue)
                colorPropID = Shader.PropertyToID("_Color");
            _renderer.GetPropertyBlock(mpBlock);
            mpBlock.SetColor(colorPropID.Value, c);
            _renderer.SetPropertyBlock(mpBlock);
        }
        #endregion
    }
}
