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
        bodyParts.ForEach(x =>
        {
            if (!x.renderer) return;
            if (x.id < 0)
            {
                x.renderer.sprite = null;
                return;
            }

            var part = data[x.type, x.id];

            x.renderer.sprite = part[BodyPartDirection.Front];
            x.renderer.sortingOrder = (int)part.layer;
            x.renderer.material.SetColor("_R", x.rMask);
            x.renderer.material.SetColor("_G", x.gMask);
            x.renderer.material.SetColor("_B", x.bMask);
        });
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
    public SpriteRenderer renderer;
    public int id;

    [Space]
    [Header("Color Masks")]
    public Color rMask;
    public Color gMask;
    public Color bMask;

#if UNITY_EDITOR
    public void OnValidate()
    {
        partName = type.ToString();
    }
#endif
}