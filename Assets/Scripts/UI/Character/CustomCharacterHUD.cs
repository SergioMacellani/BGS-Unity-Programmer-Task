using System;
using System.Linq;
using BGS.Character;
using BGS.Data;
using BGS.IO;
using UnityEngine;
using UnityEngine.UI;

namespace BGS.UI.Character
{
    /// <summary>
    /// This class is responsible for managing the HUD of a custom character.
    /// </summary>
    public class CustomCharacterHUD : MonoBehaviour
    {
        #region Variables
        
        [Header("References")]
        [SerializeField]
        [Tooltip("Reference to the custom character.")]
        private CustomCharacter customCharacter;

        [SerializeField] 
        [Tooltip("Reference to the color picker.")]
        private ColorPicker colorPicker;
        [SerializeField] 
        [Tooltip("Reference to the window manager.")]
        private WindowManager windowManager;

        [Space] 
        [Header("Color Pickers")] 
        [SerializeField]
        [Tooltip("Image for the red color picker.")]
        private Image rColor;
        [SerializeField] 
        [Tooltip("Image for the green color picker.")]
        private Image gColor;
        [SerializeField] 
        [Tooltip("Image for the blue color picker.")]
        private Image bColor;

        [Space] 
        [Header("Body Parts")] 
        [SerializeField]
        [Tooltip("The index of the current page.")]
        private BodyPartType pageIndex;

        [SerializeField] 
        [Tooltip("Prefab for body part selection item.")]
        private BodyPartSelect bodyPartItemPrefab;
        [SerializeField] 
        [Tooltip("Container for body part items.")]
        private Transform bodyPartItemContainer;
        
        #endregion

        #region Unity Callbacks

        /// <summary>
        /// Changes the page of body parts.
        /// Also sets up the color picker buttons.
        /// </summary>
        private void Start()
        {
            ChangePage(0);

            rColor.GetComponent<Button>().onClick.AddListener(() => OpenColorPicker(0));
            gColor.GetComponent<Button>().onClick.AddListener(() => OpenColorPicker(1));
            bColor.GetComponent<Button>().onClick.AddListener(() => OpenColorPicker(2));
        }

        /// <summary>
        /// Updates the body part items when the canvas group changes.
        /// </summary>
        private void OnCanvasGroupChanged()
        {
            UpdateBodyPartItems();
        }

        #endregion

        #region Public Methods
        
        /// <summary>
        /// Changes the page of body parts.
        /// </summary>
        /// <param name="index">The index of the page to change to.</param>
        public void ChangePage(int index)
        {
            pageIndex = (BodyPartType)index;
            rColor.color = customCharacter.GetColor(pageIndex, 0);
            gColor.color = customCharacter.GetColor(pageIndex, 1);
            bColor.color = customCharacter.GetColor(pageIndex, 2);
            UpdateBodyPartItems();
        }
        
        /// <summary>
        /// Saves the current state of the character.
        /// </summary>
        public void SaveCharacter()
        {
            CanvasManager.Instance.ChangeWindow(0);

            IOSystem.SaveFile(JsonUtility.ToJson(PlayerInventoryData.Instance), "data");
            JsonUtility.FromJsonOverwrite(IOSystem.LoadFile("data"), PlayerInventoryData.Instance);
        }
        
        /// <summary>
        /// Closes the color picker.
        /// </summary>
        public void CloseColorPicker()
        {
            windowManager.ChangeWindow(0);
            colorPicker.ColorChanged -= ChangeColor;
        }
        
        #endregion
        
        #region Private Methods
        
        /// <summary>
        /// Opens the color picker for a specific color.
        /// </summary>
        /// <param name="colorIndex">The index of the color to open the picker for.</param>
        private void OpenColorPicker(int colorIndex)
        {
            windowManager.ChangeWindow(1);
            colorPicker.SetUpColorPicker(colorIndex, colorIndex switch
            {
                0 => rColor.color,
                1 => gColor.color,
                2 => bColor.color,
                _ => Color.white
            });
            colorPicker.ColorChanged += ChangeColor;
        }
        
        /// <summary>
        /// Updates the body part items in the HUD.
        /// </summary>
        private void UpdateBodyPartItems()
        {
            foreach (Transform child in bodyPartItemContainer)
                Destroy(child.gameObject);

            uint id = 0;
            var data = BodyPartsData.Instance[pageIndex];
            if (data == null || data.Count == 0) return;
            foreach (var bodyPart in PlayerInventoryData.Instance.GetOwnedBodyParts(pageIndex).Select(i => data[i]))
            {
                var bodyPartItem = Instantiate(bodyPartItemPrefab, bodyPartItemContainer);
                var idInstantiate = id;
                void ChangeBodyPartAction()
                {
                    customCharacter.ChangeBodyPart((uint)PlayerInventoryData.Instance.GetOwnedBodyParts(pageIndex)[(int)idInstantiate], pageIndex);
                } ;
                Sprite bodyPartSprite = bodyPart.bodyPartSprite;
                float posYInHUD = bodyPart.posYInHUD;

                if (bodyPart is BodyPartsData.BodyPartPairs pairs)
                    bodyPartItem.SetUp(bodyPartSprite, ChangeBodyPartAction, posYInHUD, pairs.bodyPartSpritePair);
                else
                    bodyPartItem.SetUp(bodyPartSprite, ChangeBodyPartAction, posYInHUD);

                id++;
            }
        }

        /// <summary>
        /// Changes the color of a body part.
        /// </summary>
        /// <param name="color">The new color to apply.</param>
        /// <param name="colorIndex">The index of the color to change.</param>
        private void ChangeColor(Color color, int colorIndex)
        {
            switch (colorIndex)
            {
                case 0:
                    rColor.color = color;
                    break;
                case 1:
                    gColor.color = color;
                    break;
                case 2:
                    bColor.color = color;
                    break;
            }

            customCharacter.ChangeBodyPartColor(pageIndex, rColor.color, gColor.color, bColor.color);
        }
        
        #endregion
    }
}