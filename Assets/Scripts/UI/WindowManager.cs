using System.Collections.Generic;
using UnityEngine;

namespace BGS.UI
{
    public class WindowManager : MonoBehaviour
    {
        [SerializeField] private int currentWindowIndex;
        [SerializeField] private int startWindowIndex;

        [SerializeField] private List<CanvasGroup> windows;

        private void Start()
        {
            currentWindowIndex = startWindowIndex;
            UpdateWindows();
        }

        public void ChangeWindow(int index)
        {
            currentWindowIndex = index;
            UpdateWindows();
        }

        private void UpdateWindows()
        {
            for (var i = 0; i < windows.Count; i++)
            {
                windows[i].alpha = i == currentWindowIndex ? 1 : 0;
                windows[i].blocksRaycasts = i == currentWindowIndex;
                windows[i].interactable = i == currentWindowIndex;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            UpdateWindows();
        }
#endif
    }
}