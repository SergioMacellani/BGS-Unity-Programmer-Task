using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditBodyPart : MonoBehaviour
{
    [SerializeField]
    private BodyPartsData bodyPartsData;
    [SerializeField]
    private CustomClothes customClothes;
    [SerializeField]
    private ColorPicker colorPicker;

    [SerializeField]
    private BodyPartSelect bodyPartItemPrefab;
    [SerializeField]
    private Transform bodyPartItemContainer;

    [SerializeField]
    private Image primaryColor, secondaryColor, tertiaryColor;

    [SerializeField] private BodyPartType pageIndex;

    public void ChangePage(int index)
    {
        pageIndex = (BodyPartType) index;
        primaryColor.color = customClothes.GetColor(pageIndex, 0);
        secondaryColor.color = customClothes.GetColor(pageIndex, 1);
        tertiaryColor.color = customClothes.GetColor(pageIndex, 2);
        UpdateBodyPartItems();
    }

    private void Start()
    {
        UpdateBodyPartItems();

        primaryColor.GetComponent<Button>().onClick.AddListener(() => OpenColorPicker(0));
        secondaryColor.GetComponent<Button>().onClick.AddListener(() => OpenColorPicker(1));
        tertiaryColor.GetComponent<Button>().onClick.AddListener(() => OpenColorPicker(2));
    }

    private void UpdateBodyPartItems()
    {
        foreach (Transform child in bodyPartItemContainer)
        {
            Destroy(child.gameObject);
        }

        uint id = 0;
        foreach (var bodyPart in bodyPartsData[pageIndex])
        {
            var bodyPartItem = Instantiate(bodyPartItemPrefab, bodyPartItemContainer);
            var idInstantiate = id;
            bodyPartItem.SetUp(idInstantiate, bodyPart.partSprite, () => customClothes.ChangeBodyPart(idInstantiate, pageIndex));
            id++;
        }
    }

    private void OpenColorPicker(int colorIndex)
    {
        colorPicker.OpenColorPicker(colorIndex, colorIndex switch
        {
            0 => primaryColor.color,
            1 => secondaryColor.color,
            2 => tertiaryColor.color,
            _ => Color.white
        });
        colorPicker.onColorChanged += ChangeColor;
    }

    public void CloseColorPicker()
    {
        colorPicker.gameObject.SetActive(false);
        colorPicker.onColorChanged -= ChangeColor;
    }

    private void ChangeColor(Color color, int colorIndex)
    {
        switch (colorIndex)
        {
            case 0:
                primaryColor.color = color;
                break;
            case 1:
                secondaryColor.color = color;
                break;
            case 2:
                tertiaryColor.color = color;
                break;
        }
        customClothes.ChangeBodyPartColor(pageIndex, primaryColor.color, secondaryColor.color, tertiaryColor.color);
    }
}
