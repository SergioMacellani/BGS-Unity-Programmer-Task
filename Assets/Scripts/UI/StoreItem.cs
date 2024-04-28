using System;
using BGS.Character;
using BGS.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BGS.UI
{
    public class StoreItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private uint id;
        [SerializeField] private Image icon, iconMirror;
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI price;
        [SerializeField] private TextMeshProUGUI buttonText;
        [SerializeField] private BodyPartType type;

        public Transform coin;

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

        public void OnPointerEnter(PointerEventData eventData)
        {
            CustomCharacter.Instance.PreviewBodyPart(id, type);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            CustomCharacter.Instance.ExitPreview(type);
        }
    }
}