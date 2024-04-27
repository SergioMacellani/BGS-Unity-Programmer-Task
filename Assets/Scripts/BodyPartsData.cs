using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public enum BodyPartType
{
    Hat,
    Accessories,
    Head,
    Beard,
    Shirt,
    Pants,
    Shoes
}

public enum BodyPartDirection
{
    Front,
    Back,
    Left,
    Right
}

[CreateAssetMenu(fileName = "BodyParts Data", menuName = "BGS/Body Parts Data")]
public class BodyPartsData : ScriptableObject
{
    [Header("Body Parts")]
    public List<BodyPart> hats;
    public List<BodyPart> accessories;
    public List<BodyPart> heads;
    [FormerlySerializedAs("clothes")] public List<BodyPart> shirt;
    public List<BodyPart> pants;
    public List<BodyPart> shoes;
    public List<BodyPart> beards;

    public BodyPart this[BodyPartType type, int id]
    {
        get
        {
            return type switch
            {
                BodyPartType.Hat => hats[id],
                BodyPartType.Accessories => accessories[id],
                BodyPartType.Head => heads[id],
                BodyPartType.Beard => beards[id],
                BodyPartType.Shirt => shirt[id],
                BodyPartType.Pants => pants[id],
                BodyPartType.Shoes => shoes[id],
                _ => null
            };
        }
    }

    [Serializable]
    public class BodyPart
    {
        [Header("Info")]
        public string bodyTag;
        [Tooltip("The layer is used to sort the body parts. Will influence sorting order in the renderer.")]
        public uint layer;
        [Tooltip("The Color Masks are used to change the color of the body part. Will influence color change HUD.")]
        public bool3 rgbMask;

        [Space]
        [Header("Sprites")]
        public Sprite backSprite;
        public Sprite frontSprite;
        public Sprite rightSprite;
        public Sprite leftSprite;

        public Sprite this[BodyPartDirection direction]
        {
            get
            {
                return direction switch
                {
                    BodyPartDirection.Front => frontSprite,
                    BodyPartDirection.Back => backSprite,
                    BodyPartDirection.Left => leftSprite,
                    BodyPartDirection.Right => rightSprite,
                    _ => null
                };
            }
        }
    }
}