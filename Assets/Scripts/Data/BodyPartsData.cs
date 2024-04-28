using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace BGS.Data
{
    /// <summary>
    /// Enum representing the different types of body parts.
    /// </summary>
    public enum BodyPartType
    {
        Hair,
        Face,
        Head,
        Torso,
        Shoulder,
        Elbow,
        Wrist,
        Pelvis,
        Leg,
        Boot
    }

    /// <summary>
    /// This class represents the data for different body parts.
    /// </summary>
    [CreateAssetMenu(fileName = "BodyParts Data", menuName = "BGS/Body Parts Data")]
    public class BodyPartsData : ScriptableObject
    {
        /// <summary>
        /// Singleton instance of the BodyPartsData class.
        /// </summary>
        public static BodyPartsData Instance { get; set; }

        #region Variables
        
        [Header("Head")] 
        public List<BodyPart> hair;
        public List<BodyPart> face;
        public List<BodyPart> head;

        [Space] 
        [Header("Torso & Pelvis")] 
        public List<BodyPart> torso;
        public List<BodyPart> pelvis;

        [Space] [Header("Shoulders")] 
        public List<BodyPartPairs> shoulder;
        public List<BodyPartPairs> elbow;
        public List<BodyPartPairs> wrist;
        
        [Space] 
        [Header("Legs")] 
        public List<BodyPartPairs> leg;
        public List<BodyPartPairs> boot;
        
        #endregion

        #region Indexers

        /// <summary>
        /// Indexer to get a specific body part by type and id.
        /// </summary>
        public BodyPart this[BodyPartType type, int id] => type switch
        {
            BodyPartType.Hair => hair[id],
            BodyPartType.Face => face[id],
            BodyPartType.Head => head[id],
            BodyPartType.Torso => torso[id],
            BodyPartType.Pelvis => pelvis[id],
            BodyPartType.Shoulder => shoulder[id],
            BodyPartType.Elbow => elbow[id],
            BodyPartType.Wrist => wrist[id],
            BodyPartType.Leg => leg[id],
            BodyPartType.Boot => boot[id],
            _ => null
        };

        /// <summary>
        /// Indexer to get a list of body parts by type.
        /// </summary>
        public List<BodyPart> this[BodyPartType type] => new List<BodyPart>((type switch
        {
            BodyPartType.Hair => hair,
            BodyPartType.Face => face,
            BodyPartType.Head => head,
            BodyPartType.Torso => torso,
            BodyPartType.Pelvis => pelvis,
            BodyPartType.Shoulder => shoulder,
            BodyPartType.Elbow => elbow,
            BodyPartType.Wrist => wrist,
            BodyPartType.Leg => leg,
            BodyPartType.Boot => boot,
            _ => null
        })!);

        #endregion

        #region Classes

        /// <summary>
        /// This class represents a single body part.
        /// </summary>
        [Serializable]
        public class BodyPart
        {

            #region Variables
            
            [Tooltip("The tag is used to identify the body part.")]
            public string bodyTag;

            [Tooltip("The price of the body part.")]
            public uint price;

            [Tooltip("The position in the HUD item frame")]
            public float posYInHUD;

            [Tooltip("The layer is used to sort the body parts. Will influence sorting order in the renderer.")]
            public uint layer;

            [Tooltip("The Sprite of the body part.")]
            public Sprite bodyPartSprite;
            
            #endregion

            #region Conversion
            
            /// <summary>
            /// Explicit conversion from BodyPart to Sprite.
            /// </summary>
            public static explicit operator Sprite(BodyPart b)
            {
                return b.bodyPartSprite;
            }

            #endregion

            #region Constructor
            
            /// <summary>
            /// Constructor for the BodyPart class.
            /// </summary>
            public BodyPart(string bodyTag, uint layer, Sprite bodyPartSprite, float posY = 0, uint price = 0)
            {
                this.bodyTag = bodyTag;
                this.layer = layer;
                this.bodyPartSprite = bodyPartSprite;
                posYInHUD = posY;
                this.price = price == 0 ? (uint)Random.Range(10, 100) : price;
            }
            
            #endregion
            
        }

        /// <summary>
        /// This class represents a pair of body parts.
        /// </summary>
        [Serializable]
        public class BodyPartPairs : BodyPart
        {
            
            #region MyRegion

            [Tooltip("The layer is used to sort the body parts. Will influence sorting order in the renderer.")]
            public uint layerPair;

            [Tooltip("The Sprite of the body part.")]
            public Sprite bodyPartSpritePair;
            
            #endregion

            #region Indexer

            /// <summary>
            /// Indexer to get a sprite by side.
            /// </summary>
            public Sprite this[int side] => side switch
            {
                0 => bodyPartSprite,
                1 => bodyPartSpritePair,
                _ => null
            };
            
            #endregion

            #region Constructor

            /// <summary>
            /// Constructor for the BodyPartPairs class.
            /// </summary>
            public BodyPartPairs(string bodyTag, uint layer, uint layerPair, Sprite bodyPartSprite,
                Sprite bodyPartSpritePair, float posY = 0, uint price = 0) : base(bodyTag, layer, bodyPartSprite, posY,
                price)
            {
                this.layerPair = layerPair;
                this.bodyPartSpritePair = bodyPartSpritePair;
            }

            /// <summary>
            /// Constructor for the BodyPartPairs class.
            /// </summary>
            public BodyPartPairs(string bodyTag, uint layer, Sprite partSprite, Sprite bodyPartSpritePair,
                float posY = 0, uint price = 0) : base(bodyTag, layer, partSprite, posY, price)
            {
                this.layerPair = layer;
                this.bodyPartSpritePair = bodyPartSpritePair;
            }
            
            #endregion

        }
        
        #endregion
    }
}