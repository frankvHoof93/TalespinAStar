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
        [Header("Components")]
        [SerializeField]
        private ParticleSystem particles;
        /// <summary>
        /// Color to set if Tile is Start-Position for Path
        /// </summary>
        [Header("Colors")]
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

        [Header("Animation")]
        [SerializeField]
        private float tweenDuration = 1f;

        /// <summary>
        /// Renderer for Tile
        /// </summary>
        private Renderer _renderer;
        /// <summary>
        /// MaterialPropertyBlock used to update Renderer-Properties
        /// </summary>
        private MaterialPropertyBlock mpBlock;
        /// <summary>
        /// Currently running Tween
        /// </summary>
        private LTDescr currTween;
        /// <summary>
        /// Property-ID for Color-Property in Shader
        /// </summary>
        private static int? colorPropID;
        #endregion

        #region Methods
        /// <summary>
        /// Visualize Tile as Part of Path
        /// </summary>
        /// <param name="indexInPath">Index of Tile in Path</param>
        /// <param name="pathLength">Total Length of Path</param>
        public void SetPathTile(int indexInPath, int pathLength)
        {
            Vector3 localPos = transform.localPosition;
            localPos.y = .1f;
            transform.localPosition = localPos;
            Color c = indexInPath == 0 ? pathStartColor : indexInPath == pathLength - 1 ? pathEndColor : pathPieceColor;
            SetColor(c);
            if (pathLength != -1)
            {
                if (currTween != null)
                {
                    LeanTween.cancel(currTween.id);
                    currTween = null;
                }
                float delay = ((float)indexInPath / pathLength) * (tweenDuration * 2f);
                currTween = transform.LeanMoveLocalY(.25f, tweenDuration).setDelay(delay).setLoopPingPong(-1);
                var particleSettings = particles.main;
                particleSettings.startColor = c;
                particles.Play();
            }
        }
        /// <summary>
        /// Clear Visualization for Tile
        /// </summary>
        public void ClearPathTile()
        {
            if (currTween != null)
            {
                LeanTween.cancel(currTween.id);
                currTween = null;
            }
            Vector3 localPos = transform.localPosition;
            localPos.y = 0f;
            transform.localPosition = localPos;
            SetColor(Color.white);
            particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
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
