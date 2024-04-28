using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BGS.Data;

namespace BGS.Character
{
    /// <summary>
    /// This class is used to manage the custom character in the game.
    /// It handles the body parts of the character and their colors.
    /// </summary>
    public class CustomCharacter : MonoBehaviour
    {
        public static CustomCharacter Instance;

        #region Variables

        [Header("References")] 
        [SerializeField]
        [Tooltip("The data of the body parts.")]
        private BodyPartsData data;

        [SerializeField]
        [Tooltip("The body parts of the character.")]
        private List<BodyPartRenderer> bodyParts;

        [Tooltip("The body part id the player had before previewing a new one.")]
        private int lastIndex;

        #endregion

        #region Unity Callbacks

        /// <summary>
        /// It initializes the instance of this class and the BodyPartsData.
        /// </summary>
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            if (BodyPartsData.Instance == null) BodyPartsData.Instance = data;
        }

        /// <summary>
        /// It initializes the body parts of the player and updates the body sprite.
        /// </summary>
        private void Start()
        {
            if (PlayerInventoryData.Instance.bodyParts == null || PlayerInventoryData.Instance.bodyParts.Count == 0)
                PlayerInventoryData.Instance.bodyParts =
                    new List<BodyPartItem>(bodyParts.Select(x => new BodyPartItem(x.id, x.type)));

            if (PlayerInventoryData.Instance.bodyPartsEquipped == null ||
                PlayerInventoryData.Instance.bodyPartsEquipped.Count == 0)
                PlayerInventoryData.Instance.bodyPartsEquipped =
                    new List<BodyPartEquipped>(bodyParts.Select(x => x.GetEquipped()));

            for (var index = 0; index < bodyParts.Count; index++)
            {
                bodyParts[index].id = PlayerInventoryData.Instance.bodyPartsEquipped[index].id;
                bodyParts[index].rMask = PlayerInventoryData.Instance.bodyPartsEquipped[index].rMask;
                bodyParts[index].gMask = PlayerInventoryData.Instance.bodyPartsEquipped[index].gMask;
                bodyParts[index].bMask = PlayerInventoryData.Instance.bodyPartsEquipped[index].bMask;
            }

            UpdateBodySprite();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Changes a body part of the character.
        /// </summary>
        public void ChangeBodyPart(uint id, BodyPartType type)
        {
            var part = data[type, (int)id];
            if (part == null) return;

            bodyParts.Find(x => x.type == type).id = (int)id;
            UpdateBodySprite();
        }

        /// <summary>
        /// Changes the color of a body part of the character.
        /// </summary>
        public void ChangeBodyPartColor(BodyPartType type, Color r, Color g, Color b)
        {
            var part = bodyParts.Find(x => x.type == type);
            part.rMask = r;
            part.gMask = g;
            part.bMask = b;
            UpdateBodySprite();
        }
        
        /// <summary>
        /// Previews a body part of the character.
        /// </summary>
        public void PreviewBodyPart(uint id, BodyPartType type)
        {
            var part = data[type, (int)id];
            if (part == null) return;

            var p = bodyParts.Find(x => x.type == type);
            lastIndex = p.id;
            p.id = (int)id;
            UpdateBodySprite();
        }

        /// <summary>
        /// Exits the preview of a body part.
        /// </summary>
        public void ExitPreview(BodyPartType type)
        {
            bodyParts.Find(x => x.type == type).id = lastIndex;
            UpdateBodySprite();
        }

        #endregion
        
        #region Private Methods
        
        /// <summary>
        /// Updates the body sprite of the character.
        /// </summary>
        private void UpdateBodySprite()
        {
            bodyParts.ForEach(x => x.UpdateRenderer(data));
        }

        #endregion

        #region Public Getters
        
        /// <summary>
        /// Gets the color of a body part of the character.
        /// </summary>
        public Color GetColor(BodyPartType pageIndex, int colorIndex)
        {
            var part = bodyParts.Find(x => x.type == pageIndex);
            return colorIndex switch
            {
                0 => new Color(part.rMask.r, part.rMask.g, part.rMask.b, 1),
                1 => new Color(part.gMask.r, part.gMask.g, part.gMask.b, 1),
                2 => new Color(part.bMask.r, part.bMask.g, part.bMask.b, 1),
                _ => Color.white
            };
        }
        
        #endregion

#if UNITY_EDITOR
        /// <summary>
        /// This function is called when the script is loaded or a value is changed in the inspector.
        /// It validates the body parts and updates the body sprite.
        /// </summary>
        public void OnValidate()
        {
            bodyParts.ForEach(x => x.OnValidate());
            UpdateBodySprite();
        }
#endif
    }

    /// <summary>
    /// This class is used to manage the rendering of the body parts of the character.
    /// </summary>
    [Serializable]
    public class BodyPartRenderer
    {

        #region Variables
        
#if UNITY_EDITOR
        [HideInInspector]
        public string partName;
#endif

        [Header("Settings")] 
        public BodyPartType type;
        public List<SpriteRenderer> renderer;
        public int id;

        [Space] 
        [Header("Color Masks")] 
        public Color rMask;
        public Color gMask;
        public Color bMask;
        
        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the renderer of the body part.
        /// </summary>
        public void UpdateRenderer(BodyPartsData data)
        {
            if (renderer == null) return;
            if (id < 0)
            {
                renderer[0].sprite = null;
                return;
            }

            var part = data[type, id];

            renderer[0].sprite = (Sprite)part;
            renderer[0].sortingOrder = (int)part.layer;
            if (part is BodyPartsData.BodyPartPairs pairs)
            {
                renderer[1].sprite = pairs[1];
                renderer[1].sortingOrder = (int)pairs.layerPair;
            }

            foreach (var r in renderer)
            {
                r.material.SetColor("_R", rMask);
                r.material.SetColor("_G", gMask);
                r.material.SetColor("_B", bMask);
            }
        }

        /// <summary>
        /// Gets the equipped body part.
        /// </summary>
        public BodyPartEquipped GetEquipped()
        {
            return new BodyPartEquipped(id, rMask, gMask, bMask);
        }
        
        #endregion
        
#if UNITY_EDITOR
        /// <summary>
        /// This function is called when the script is loaded or a value is changed in the inspector.
        /// It validates the name of the body part.
        /// </summary>
        public void OnValidate()
        {
            partName = type.ToString();
        }
#endif
    }
}