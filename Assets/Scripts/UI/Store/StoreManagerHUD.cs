using System.Linq;
using BGS.Character;
using BGS.Data;
using BGS.IO;
using TMPro;
using UnityEngine;

namespace BGS.UI.Store
{
    /// <summary>
    /// This class manages the store HUD in the game.
    /// </summary>
    public class StoreManagerHUD : MonoBehaviour
    {
        #region Variable

        [SerializeField] 
        [Tooltip("The title of the store")]
        private TextMeshProUGUI storeTitle;
        [SerializeField] 
        [Tooltip("The prefab for the store item")]
        private StoreItem storeItemPrefab;
        [SerializeField] 
        [Tooltip("The container for the store items")]
        private Transform storeItemsParent;

        [Tooltip("The type of body part")]
        private BodyPartType _type;
        
        #endregion

        #region Public Methods
        
        /// <summary>
        /// Sets up the store items in the HUD based on the type of body part.
        /// </summary>
        /// <param name="type">The type of body part.</param>
        public void SetUpStoreItems(BodyPartType type)
        {
            this._type = type;
            storeTitle.text = $"{type} Store";

            foreach (Transform child in storeItemsParent)
                Destroy(child.gameObject);

            uint id = 0;
            var data = BodyPartsData.Instance[type];
            if (data == null || data.Count == 0) return;
            
            foreach (var bodyPart in data)
            {
                var storeItem = Instantiate(storeItemPrefab, storeItemsParent);
                var isPurchased = PlayerInventoryData.Instance.GetOwnedBodyParts(type).Contains((int)id);

                storeItem.SetUp(id, bodyPart.bodyPartSprite, bodyPart.price, isPurchased, type,
                    isPurchased ? SellItem : BuyItem, bodyPart.posYInHUD, 
                    bodyPart is BodyPartsData.BodyPartPairs pairs ? pairs.bodyPartSpritePair : null);

                id++;
            }
        }

        #endregion

        #region Private Methods
        
        /// <summary>
        /// Buys an item from the store.
        /// </summary>
        /// <param name="id">The id of the item to buy.</param>
        private void BuyItem(uint id)
        {
            // Get the body part from the store and check if the player has enough coins
            var bodyPart = BodyPartsData.Instance[_type][(int)id];
            if (PlayerInventoryData.Instance.coins < bodyPart.price) return;

            // Subtract the coins by the price of the body part
            CanvasManager.Instance.AddCoins(-(int)bodyPart.price);
            // Add the body part to the inventory
            PlayerInventoryData.Instance.bodyParts.Add(new BodyPartItem((int)id, _type));
            // Update the store items
            SetUpStoreItems(_type);

            // Save the data
            IOSystem.SaveFile(JsonUtility.ToJson(PlayerInventoryData.Instance), "data");
            JsonUtility.FromJsonOverwrite(IOSystem.LoadFile("data"), PlayerInventoryData.Instance);
        }

        /// <summary>
        /// Sells an item from the store.
        /// </summary>
        /// <param name="id">The id of the item to sell.</param>
        private void SellItem(uint id)
        {
            var bodyPart = BodyPartsData.Instance[_type][(int)id];
            
            // If there is only one body part of this type, don't sell it
            if (PlayerInventoryData.Instance.bodyParts.FindAll(x => x.type == _type).Count <= 1) return;

            // Increase the coins by the price of the body part
            CanvasManager.Instance.AddCoins((int)bodyPart.price,
                storeItemsParent.GetChild((int)id).GetComponent<StoreItem>().coin.position + new Vector3(75, 0, 0));
            // Remove the body part from the inventory
            PlayerInventoryData.Instance.bodyParts.Remove(
                PlayerInventoryData.Instance.bodyParts.Find(i => i.id == id && i.type == _type));
            // Change the body part to the default one
            CustomCharacter.Instance.ChangeBodyPart(
                (uint)PlayerInventoryData.Instance.bodyParts.First(i => i.type == _type).id, _type);
            // Update the store items
            SetUpStoreItems(_type);

            // Save the data
            IOSystem.SaveFile(JsonUtility.ToJson(PlayerInventoryData.Instance), "data");
            JsonUtility.FromJsonOverwrite(IOSystem.LoadFile("data"), PlayerInventoryData.Instance);
        }
        
        #endregion
    }
}