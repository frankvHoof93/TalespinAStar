using Talespin.AStar.GameMap;
using Talespin.AStar.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Talespin.AStar.UI
{
    public class GameplayUI : CachedBehaviour<GameplayUI>
    {
        [Header("Grid-Generation")]
        [SerializeField]
        private TMP_InputField ifGridWidth;
        [SerializeField]
        private TMP_InputField ifGridHeight;
        [SerializeField]
        private Button btnGenerateGrid;

        private void Start()
        {
            MapManager mgr = MapManager.Instance;
            ifGridWidth.text = mgr.MapSize.x.ToString();
            ifGridHeight.text = mgr.MapSize.y.ToString();
            btnGenerateGrid.onClick.AddListener(() => mgr.SpawnMap(uint.Parse(ifGridWidth.text), uint.Parse(ifGridHeight.text)));
        }
        
    }
}
