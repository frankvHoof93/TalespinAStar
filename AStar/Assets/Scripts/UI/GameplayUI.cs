using Talespin.AStar.GameMap;
using Talespin.AStar.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Talespin.AStar.UI
{
    /// <summary>
    /// Manages UI-Canvas in Game
    /// </summary>
    public class GameplayUI : CachedBehaviour<GameplayUI>
    {
        #region Properties
        /// <summary>
        /// InputField for Width of Grid
        /// </summary>
        [Header("Grid-Generation")]
        [SerializeField]
        [Tooltip("InputField for Width of Grid")]
        private TMP_InputField ifGridWidth;
        /// <summary>
        /// InputField for Height of Grid
        /// </summary>
        [SerializeField]
        [Tooltip("InputField for Height of Grid")]
        private TMP_InputField ifGridHeight;
        /// <summary>
        /// Button for Generating (new) Grid
        /// </summary>
        [SerializeField]
        [Tooltip("Button for Generating (new) Grid")]
        private Button btnGenerateGrid;
        #endregion

        #region Methods
        /// <summary>
        /// Initializes UI
        /// </summary>
        private void Start()
        {
            MapManager mgr = MapManager.Instance;
            ifGridWidth.text = mgr.MapSize.x.ToString();
            ifGridWidth.onEndEdit.AddListener(CheckWidth);
            ifGridHeight.text = mgr.MapSize.y.ToString();
            ifGridHeight.onEndEdit.AddListener(CheckHeight);
            btnGenerateGrid.onClick.AddListener(() => mgr.SpawnMap(uint.Parse(ifGridWidth.text), uint.Parse(ifGridHeight.text)));
        }
        /// <summary>
        /// Checks if Width is a 'Valid' value
        /// </summary>
        /// <param name="newValue">Value set to InputField</param>
        private void CheckWidth(string newValue)
        {
            int width = int.Parse(newValue);
            if (width < 3)
            {
                Debug.LogWarning("Too Small Width. Set Width to 3 instead");
                ifGridWidth.SetTextWithoutNotify(3.ToString());
            }
            else if (width > 125)
            {
                Debug.LogWarning("Too Large Width. Set Width to 125 instead");
                ifGridWidth.SetTextWithoutNotify(125.ToString());
            }
        }
        /// <summary>
        /// Checks if Height is a 'Valid' value
        /// </summary>
        /// <param name="newValue">Value set to InputField</param>
        private void CheckHeight(string newValue)
        {
            int height = int.Parse(newValue);
            if (height < 3)
            {
                Debug.LogWarning("Too Small Height. Set Height to 3 instead");
                ifGridHeight.SetTextWithoutNotify(3.ToString());
            }
            else if (height > 125)
            {
                Debug.LogWarning("Too Large Height. Set Height to 125 instead");
                ifGridHeight.SetTextWithoutNotify(125.ToString());
            }
        }
        #endregion
    }
}
