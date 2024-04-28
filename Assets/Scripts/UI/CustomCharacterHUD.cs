using System.Linq;
using BGS.Character;
using BGS.Data;
using BGS.IO;
using UnityEngine;
using UnityEngine.UI;

namespace BGS.UI
{
    public class CustomCharacterHUD : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private CustomCharacter customCharacter;

        [SerializeField] private ColorPicker colorPicker;
        [SerializeField] private WindowManager windowManager;

        [Space] [Header("Color Pickers")] [SerializeField]
        private Image rColor;

        [SerializeField] private Image gColor;
        [SerializeField] private Image bColor;

        [Space] [Header("Body Parts")] [SerializeField]
        private BodyPartType pageIndex;

        [SerializeField] private BodyPartSelect bodyPartItemPrefab;
        [SerializeField] private Transform bodyPartItemContainer;

        private void Start()
        {
            ChangePage(0);

            rColor.GetComponent<Button>().onClick.AddListener(() => OpenColorPicker(0));
            gColor.GetComponent<Button>().onClick.AddListener(() => OpenColorPicker(1));
            bColor.GetComponent<Button>().onClick.AddListener(() => OpenColorPicker(2));
        }

        public void ChangePage(int index)
        {
            pageIndex = (BodyPartType)index;
            rColor.color = customCharacter.GetColor(pageIndex, 0);
            gColor.color = customCharacter.GetColor(pageIndex, 1);
            bColor.color = customCharacter.GetColor(pageIndex, 2);
            UpdateBodyPartItems();
        }

        public void SaveCharacter()
        {
            CanvasManager.Instance.ChangeWindow(0);

            IOSystem.SaveFile(JsonUtility.ToJson(PlayerInventoryData.Instance), "data");
            JsonUtility.FromJsonOverwrite(IOSystem.LoadFile("data"), PlayerInventoryData.Instance);
        }

        private void UpdateBodyPartItems()
        {
            foreach (Transform child in bodyPartItemContainer)
            {
                Destroy(child.gameObject);
            }

            uint id = 0;
            var data = BodyPartsData.Instance[pageIndex];
            if (data == null || data.Count == 0) return;
            foreach (var bodyPart in PlayerInventoryData.Instance.GetOwnedBodyParts(pageIndex).Select(i => data[i]))
            {
                var bodyPartItem = Instantiate(bodyPartItemPrefab, bodyPartItemContainer);
                var idInstantiate = id;
                if (bodyPart is BodyPartsData.BodyPartPairs pairs)
                    bodyPartItem.SetUp(bodyPart.bodyPartSprite,
                        () => customCharacter.ChangeBodyPart(idInstantiate, pageIndex),
                        bodyPart.posYInHUD,
                        pairs.bodyPartSpritePair);
                else
                    bodyPartItem.SetUp(bodyPart.bodyPartSprite,
                        () => customCharacter.ChangeBodyPart(idInstantiate, pageIndex),
                        bodyPart.posYInHUD);

                id++;
            }
        }

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
            colorPicker.onColorChanged += ChangeColor;
        }

        public void CloseColorPicker()
        {
            windowManager.ChangeWindow(0);
            colorPicker.onColorChanged -= ChangeColor;
        }

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
    }
}