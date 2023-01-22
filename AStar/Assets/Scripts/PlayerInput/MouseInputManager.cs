using System;
using Talespin.AStar.GameMap.MapTiles;
using Talespin.AStar.Utils;
using UnityEngine;

namespace Talespin.AStar.PlayerInput
{
    public class MouseInputManager : CachedBehaviour<MouseInputManager>
    {
        public delegate void HandleTileChange(Tile previousTile, Tile newTile);

        private const float MAX_DISTANCE = 10_000f;

        public event HandleTileChange OnStartTileChanged;
        public event HandleTileChange OnEndTileChanged;
        public event Action OnSelectionChanged;


        public Tile StartTile { get; private set; } = null;
        public Tile EndTile { get; private set; } = null;

        [SerializeField]
        private LayerMask tileLayer;
        [SerializeField]
        private Camera cam;

        protected override void Awake()
        {
            base.Awake();
            if (instance == this)
            {
                if (cam == null)
                    cam = Camera.main;
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                TrySelectStart();
            if (Input.GetMouseButtonDown(1))
                TrySelectEnd();
        }

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

        private Tile TrySelectTile()
        {
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, MAX_DISTANCE, tileLayer))
                return hit.collider.GetComponent<Tile>();
            return null;
        }
    }
}
