using System;
using BGS.Character;
using BGS.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BGS.UI.Store
{
    /// <summary>
    /// This class represents a store item in the BGS game.
    /// It handles the UI representation and interaction of a store item.
    /// </summary>
    public class StoreItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region Variables

        [SerializeField] 
        [Tooltip("The ID of the store item")]
        private uint id;
        [SerializeField]
        [Tooltip("The icon and mirror icon of the store item")]
        private Image icon, iconMirror;
        [SerializeField] 
        [Tooltip("The button for purchasing or selling the store item")]
        private Button button; 
        [SerializeField] 
        [Tooltip("The price of the store item")]
        private TextMeshProUGUI price;
        [SerializeField] 
        [Tooltip("The text of the button")]
        private TextMeshProUGUI buttonText;
        [SerializeField] 
        [Tooltip("The type of the body part the store item represents")]
        private BodyPartType type;

        [Tooltip("The coin used for purchasing the store item")]
        public Transform coin; 
        
        #endregion

        #region Public Methods

        /// <summary>
        /// Sets up the store item with the given parameters.
        /// </summary>
        public void SetUp(uint id, Sprite icon, uint price, bool isPurchased, BodyPartType type, Action<uint> onClick,
            float posY = 0, Sprite iconMirror = null)
        {
            this.id = id;
            this.icon.sprite = icon;
            this.icon.rectTransform.anchoredPosition = new Vector2(0, posY);
            this.price.text = price.ToString();
            this.type = type;

            buttonText.text = isPurchased ? "Sell" : "Buy";

            if (iconMirror == null) this.iconMirror.gameObject.SetActive(false);
            else this.iconMirror.sprite = iconMirror;

            button.onClick.AddListener(() => onClick(this.id));
        }
        
        #endregion

        #region Event Handlers
        
        /// <summary>
        /// Handles the event when the pointer enters the store item.
        /// </summary>
        public void OnPointerEnter(PointerEventData eventData)
        {
            CustomCharacter.Instance.PreviewBodyPart(id, type);
        }

        /// <summary>
        /// Handles the event when the pointer exits the store item.
        /// </summary>
        public void OnPointerExit(PointerEventData eventData)
        {
            CustomCharacter.Instance.ExitPreview(type);
        }
        
        #endregion
    }
}