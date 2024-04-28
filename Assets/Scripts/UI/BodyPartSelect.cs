using System;
using UnityEngine;
using UnityEngine.UI;

namespace BGS.UI
{
    public class BodyPartSelect : MonoBehaviour
    {
        #region Variables

        [SerializeField] private Image icon, iconMirror;
        [SerializeField] private Button button;
        
        #endregion

        #region Public Methods   
        
        public void SetUp(Sprite icon, Action onClick, float posY = 0, Sprite iconMirror = null)
        {
            this.icon.sprite = icon;
            this.icon.rectTransform.anchoredPosition = new Vector2(0, posY);

            if (iconMirror == null) this.iconMirror.gameObject.SetActive(false);
            else this.iconMirror.sprite = iconMirror;

            button.onClick.AddListener(() => onClick());
        }
        
        #endregion

    }
}