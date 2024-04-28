using System.Collections.Generic;
using UnityEngine;

namespace BGS.UI
{
    /// <summary>
    /// This class manages the different windows in the game UI.
    /// </summary>
    public class WindowManager : MonoBehaviour
    {
        #region Variables
 
        [SerializeField] 
        [Tooltip("The index of the current window.")]
        private int currentWindowIndex;
        [SerializeField] 
        [Tooltip("The index of the window to start with.")]
        private int startWindowIndex;

        [SerializeField] 
        [Tooltip("The list of windows to manage.")]
        private List<CanvasGroup> windows;
        
        #endregion

        #region Unity Callbacks

        /// <summary>
        /// Sets the current window index to the start window index and updates the windows at the start of the game.
        /// </summary>
        private void Start()
        {
            currentWindowIndex = startWindowIndex;
            UpdateWindows();
        }
        
        #endregion

        #region Public Methods

        /// <summary>
        /// Changes the current window to the window at the given index.
        /// </summary>
        /// <param name="index">The index of the window to change to.</param>
        public void ChangeWindow(int index)
        {
            currentWindowIndex = index;
            UpdateWindows();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates the windows' visibility and interactivity based on the current window index.
        /// </summary>
        private void UpdateWindows()
        {
            for (var i = 0; i < windows.Count; i++)
            {
                windows[i].alpha = i == currentWindowIndex ? 1 : 0;
                windows[i].blocksRaycasts = i == currentWindowIndex;
                windows[i].interactable = i == currentWindowIndex;
            }
        }

        #endregion

        #region Unity Editor Only

#if UNITY_EDITOR
        /// <summary>
        /// Updates the windows when the script is changed in the editor.
        /// </summary>
        private void OnValidate()
        {
            UpdateWindows();
        }
#endif
        
        #endregion
    }
}