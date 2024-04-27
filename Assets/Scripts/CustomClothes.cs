using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomClothes : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private BodyPartsData data;
    [SerializeField]
    private List<BodyPartRenderer> bodyParts;

    public void UpdateBodySprite()
    {
        bodyParts.ForEach(x => x.UpdateRenderer(data));
    }

    public void ChangeBodyPart(uint id, BodyPartType type)
    {
        var part = data[type, (int)id];
        if (part == null) return;

        bodyParts.Find(x => x.type == type).id = (int)id;
        UpdateBodySprite();
    }

    public void ChangeBodyPartColor(BodyPartType type, Color r, Color g, Color b)
    {
        var part = bodyParts.Find(x => x.type == type);
        part.rMask = r;
        part.gMask = g;
        part.bMask = b;
        UpdateBodySprite();
    }

    public Color GetColor(BodyPartType pageIndex, int p1)
    {
        var part = bodyParts.Find(x => x.type == pageIndex);
        switch (p1)
        {
            case 0:
                return part.rMask;
            case 1:
                return part.gMask;
            case 2:
                return part.bMask;
            default:
                return Color.white;
        }


    }

#if UNITY_EDITOR
    public void OnValidate()
    {
        bodyParts.ForEach(x => x.OnValidate());
        UpdateBodySprite();
    }
#endif
}

[Serializable]
public class BodyPartRenderer
{
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

    public void UpdateRenderer(BodyPartsData data)
    {
        if (renderer == null) return;
        if (id < 0)
        {
            renderer[0].sprite = null;
            return;
        }

        var part = data[type, id];

        Debug.Log(type);
        if (part is BodyPartsData.BodyPartMirror mirror)
        {
            renderer[0].sprite = mirror[0];
            renderer[1].sprite = mirror[1];

            renderer[0].sortingOrder = (int)part.layer;
            renderer[1].sortingOrder = (int)mirror.layerLeft;
        }else
        {
            renderer[0].sprite = (Sprite)part;
            renderer[0].sortingOrder = (int)part.layer;
        }

        foreach (var r in renderer)
        {
            r.material.SetColor("_R", rMask);
            r.material.SetColor("_G", gMask);
            r.material.SetColor("_B", bMask);
        }
    }

#if UNITY_EDITOR
    public void OnValidate()
    {
        partName = type.ToString();
    }
#endif
}