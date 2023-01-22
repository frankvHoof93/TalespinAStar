using System;
using Talespin.AStar.GameMap.MapTiles;
using Talespin.AStar.UI;
using Talespin.AStar.Utils;
using UnityEngine;

namespace Talespin.AStar.PlayerInput
{
    /// <summary>
    /// Handles Mouse-Input for Player (Tile-Selection)
    /// </summary>
    public class MouseInputManager : CachedBehaviour<MouseInputManager>
    {
        #region Constants
        /// <summary>
        /// Max Raycast-Distance for Selection
        /// </summary>
        private const float MAX_DISTANCE = 1_000f;
        #endregion

        #region Events
        /// <summary>
        /// Delegate for Events Fired if a Selected Tile changes
        /// </summary>
        /// <param name="previousTile">Formerly Selected Tile</param>
        /// <param name="newTile">Newly Selected Tile</param>
        public delegate void HandleTileChange(Tile previousTile, Tile newTile);

        /// <summary>
        /// Even Fired when a new Start-Tile is Selected
        /// </summary>
        public event HandleTileChange OnStartTileChanged;
        /// <summary>
        /// Even Fired when a new End-Tile is Selected
        /// </summary>
        public event HandleTileChange OnEndTileChanged;
        /// <summary>
        /// Even Fired when a new Tile is Selected (Start or End)
        /// </summary>
        public event Action OnSelectionChanged;
        #endregion

        #region Properties
        /// <summary>
        /// Currently Selected Start-Tile for Path
        /// </summary>
        public Tile StartTile { get; private set; } = null;
        /// <summary>
        /// Currently Selected End-Tile for Path
        /// </summary>
        public Tile EndTile { get; private set; } = null;

        /// <summary>
        /// LayerMask for Tiles
        /// </summary>
        [SerializeField]
        [Tooltip("LayerMask for Tiles")]
        private LayerMask tileLayer;
        /// <summary>
        /// Camera used for Raycasting
        /// </summary>
        [SerializeField]
        [Tooltip("Camera used for Raycasting")]
        private Camera cam;
        #endregion

        #region Methods
        #region Unity
        /// <summary>
        /// Self-Initialization
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            if (instance == this)
            {
                if (cam == null)
                    cam = CameraManager.Instance.MainCam;
            }
        }
        /// <summary>
        /// Tries Selecting Tiles upon Input
        /// </summary>
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                TrySelectStart();
            if (Input.GetMouseButtonDown(1))
                TrySelectEnd();
        }
        #endregion

        #region Private
        /// <summary>
        /// Tries Selecting a new Start-Tile
        /// </summary>
        private void TrySelectStart()
        {
            Tile t = TrySelectTile();
            if (t != null && t != StartTile)
            {
                if (t == EndTile)
                {
                    Debug.LogWarning("Can\'t pick Start- & End-Tile to be the same Tile.");
                    return; // Ignore
                }
                OnStartTileChanged?.Invoke(StartTile, t);
                StartTile = t;
                OnSelectionChanged?.Invoke();
            }
        }
        /// <summary>
        /// Tries Selecting a new End-Tile
        /// </summary>
        private void TrySelectEnd()
        {
            Tile t = TrySelectTile();
            if (t != null && t != EndTile)
            {
                if (t == StartTile)
                {
                    Debug.LogWarning("Can\'t pick Start- & End-Tile to be the same Tile.");
                    return; // Ignore
                }
                OnEndTileChanged?.Invoke(EndTile, t);
                EndTile = t;
                OnSelectionChanged?.Invoke();
            }
        }
        /// <summary>
        /// Raycasts for a Tile
        /// </summary>
        /// <returns>Tile if Raycast hit. NULL otherwise</returns>
        private Tile TrySelectTile()
        {
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, MAX_DISTANCE, tileLayer))
                return hit.collider.GetComponent<Tile>();
            return null;
        }
        #endregion
        #endregion
    }
}
