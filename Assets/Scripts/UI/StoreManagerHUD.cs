using System.Linq;
using BGS.Character;
using BGS.Data;
using BGS.IO;
using TMPro;
using UnityEngine;

namespace BGS.UI
{
    public class StoreManagerHUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI storeTitle;
        [SerializeField] private StoreItem storeItemPrefab;
        [SerializeField] private Transform storeItemsParent;

        private BodyPartType type;

        public void SetUpStoreItems(BodyPartType type)
        {
            this.type = type;

            storeTitle.text = $"{type} Store";

            foreach (Transform child in storeItemsParent)
            {
                Destroy(child.gameObject);
            }

            uint id = 0;
            var data = BodyPartsData.Instance[type];
            if (data == null || data.Count == 0) return;
            foreach (var bodyPart in data)
            {
                var storeItem = Instantiate(storeItemPrefab, storeItemsParent);
                var idInstantiate = id;
                var isPurchased = PlayerInventoryData.Instance.GetOwnedBodyParts(type).Contains((int)id);
                if (bodyPart is BodyPartsData.BodyPartPairs pairs)
                    storeItem.SetUp(idInstantiate, bodyPart.bodyPartSprite, bodyPart.price, isPurchased, type,
                        isPurchased ? SellItem : BuyItem, bodyPart.posYInHUD, pairs.bodyPartSpritePair);
                else
                    storeItem.SetUp(idInstantiate, bodyPart.bodyPartSprite, bodyPart.price, isPurchased, type,
                        isPurchased ? SellItem : BuyItem, bodyPart.posYInHUD);

                id++;
            }
        }

        private void BuyItem(uint id)
        {
            var bodyPart = BodyPartsData.Instance[type][(int)id];
            if (PlayerInventoryData.Instance.coins < bodyPart.price) return;

            CanvasManager.Instance.AddCoins(-(int)bodyPart.price);
            PlayerInventoryData.Instance.bodyParts.Add(new BodyPartItem((int)id, type));
            SetUpStoreItems(type);

            IOSystem.SaveFile(JsonUtility.ToJson(PlayerInventoryData.Instance), "data");
            JsonUtility.FromJsonOverwrite(IOSystem.LoadFile("data"), PlayerInventoryData.Instance);
        }

        private void SellItem(uint id)
        {
            var bodyPart = BodyPartsData.Instance[type][(int)id];
            if (PlayerInventoryData.Instance.bodyParts.FindAll(x => x.type == type).Count <= 1) return;

            CanvasManager.Instance.AddCoins((int)bodyPart.price,
                storeItemsParent.GetChild((int)id).GetComponent<StoreItem>().coin.position + new Vector3(75, 0, 0));
            PlayerInventoryData.Instance.bodyParts.Remove(
                PlayerInventoryData.Instance.bodyParts.Find(i => i.id == id && i.type == type));
            CustomCharacter.Instance.ChangeBodyPart(
                (uint)PlayerInventoryData.Instance.bodyParts.First(i => i.type == type).id, type);
            SetUpStoreItems(type);

            IOSystem.SaveFile(JsonUtility.ToJson(PlayerInventoryData.Instance), "data");
            JsonUtility.FromJsonOverwrite(IOSystem.LoadFile("data"), PlayerInventoryData.Instance);
        }
    }
}