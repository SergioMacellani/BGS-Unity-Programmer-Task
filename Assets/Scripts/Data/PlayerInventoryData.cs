using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BGS.Data
{
    /// <summary>
    /// This class is used to manage the player's inventory data in the game.
    /// It handles the body parts and coins of the player.
    /// </summary>
    [CreateAssetMenu(fileName = "Player Inventory Data", menuName = "BGS/Player Inventory Data")]
    public class PlayerInventoryData : ScriptableObject
    {
        /// <summary>
        /// Singleton instance of the PlayerInventoryData.
        /// </summary>
        public static PlayerInventoryData Instance { get; set; }

        [Tooltip("The number of coins the player has.")]
        public uint coins;

        [Tooltip("The list of body parts the player has unlocked.")]
        public List<BodyPartItem> bodyParts = new List<BodyPartItem>();

        [Tooltip("The list of body parts the player has equipped.")]
        public List<BodyPartEquipped> bodyPartsEquipped = new List<BodyPartEquipped>();

        /// <summary>
        /// Returns a list of IDs of the owned body parts of a certain type.
        /// </summary>
        public List<int> GetOwnedBodyParts(BodyPartType type)
        {
            return (from t in bodyParts where t.type == type select t.id).ToList();
        }

        /// <summary>
        /// This function is called when the script is loaded or a value is changed in the inspector.
        /// It validates the body parts and removes duplicates.
        /// </summary>
        private void OnValidate()
        {
#if UNITY_EDITOR
        // Remove the possibility of having the same body part twice modifying the list in unity editor
        for (int i = 0; i < bodyParts.Count; i++)
        {
            for (int j = i + 1; j < bodyParts.Count; j++)
            {
                if (bodyParts[i].type == bodyParts[j].type && bodyParts[i].id == bodyParts[j].id)
                {
                    bodyParts.RemoveAt(j);
                    j--;
                }
            }
        }
#endif
        }
    }

    /// <summary>
    /// This class represents a body part item in the player's inventory.
    /// </summary>
    [Serializable]
    public class BodyPartItem
    {
        public BodyPartType type;
        public int id;

        /// <summary>
        /// Constructor for the BodyPartItem class.
        /// </summary>
        public BodyPartItem(int id, BodyPartType type)
        {
            this.id = id;
            this.type = type;
        }
    }

    /// <summary>
    /// This class represents a body part that the player has equipped.
    /// </summary>
    [Serializable]
    public class BodyPartEquipped
    {
        public int id;
        public Color rMask;
        public Color gMask;
        public Color bMask;

        /// <summary>
        /// Constructor for the BodyPartEquipped class.
        /// </summary>
        public BodyPartEquipped(int id, Color rMask, Color gMask, Color bMask)
        {
            this.id = id;
            this.rMask = rMask;
            this.gMask = gMask;
            this.bMask = bMask;
        }
    }
}