using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

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

[CreateAssetMenu(fileName = "BodyParts Data", menuName = "BGS/Body Parts Data")]
public class BodyPartsData : ScriptableObject
{
    [Header("Head")]
    public List<BodyPart> Hair;
    public List<BodyPart> Face;
    public List<BodyPart> Head;

    [Space]
    [Header("Torso & Pelvis")]
    public List<BodyPart> Torso;
    public List<BodyPart> Pelvis;

    [Space]
    [Header("Shoulders")]
    public List<BodyPartMirror> Shoulder;
    public List<BodyPartMirror> Elbow;
    public List<BodyPartMirror> Wrist;

    [Space]
    [Header("Legs")]
    public List<BodyPartMirror> Leg;
    public List<BodyPartMirror> Boot;

    public BodyPart this[BodyPartType type, int id] => type switch
    {
        BodyPartType.Hair => Hair[id],
        BodyPartType.Face => Face[id],
        BodyPartType.Head => Head[id],
        BodyPartType.Torso => Torso[id],
        BodyPartType.Pelvis => Pelvis[id],
        BodyPartType.Shoulder => Shoulder[id],
        BodyPartType.Elbow => Elbow[id],
        BodyPartType.Wrist => Wrist[id],
        BodyPartType.Leg => Leg[id],
        BodyPartType.Boot => Boot[id],
        _ => null
    };

    public List<BodyPart> this[BodyPartType type] => type switch
    {
        BodyPartType.Hair => Hair,
        BodyPartType.Face => Face,
        BodyPartType.Head => Head,
        BodyPartType.Torso => Torso,
        BodyPartType.Pelvis => Pelvis,
        _ => null
    };

    [Serializable]
    public class BodyPart
    {
        [Tooltip("The tag is used to identify the body part.")]
        public string bodyTag;
        [Tooltip("The Color Masks are used to change the color of the body part. Will influence color change HUD.")]
        public bool3 rgbMask;

        [Tooltip("The layer is used to sort the body parts. Will influence sorting order in the renderer.")]
        public uint layer;
        [Tooltip("The Sprite of the body part.")]
        public Sprite partSprite;

        public static explicit operator Sprite(BodyPart b)
        {
            return b.partSprite;
        }

        public BodyPart(string bodyTag, uint layer, Sprite partSprite)
        {
            this.bodyTag = bodyTag;
            this.layer = layer;
            this.partSprite = partSprite;
        }
    }

    [Serializable]
    public class BodyPartMirror : BodyPart
    {
        [Tooltip("The layer is used to sort the body parts. Will influence sorting order in the renderer.")]
        public uint layerLeft;
        [Tooltip("The Sprite of the body part.")]
        public Sprite partSpriteLeft;

        public Sprite this[int side] => side switch
        {
            0 => partSprite,
            1 => partSpriteLeft,
            _ => null
        };

        public BodyPartMirror(string bodyTag, uint layer, uint layerLeft, Sprite partSprite, Sprite partSpriteLeft) : base(bodyTag, layer, partSprite)
        {
            this.layerLeft = layerLeft;
            this.partSpriteLeft = partSpriteLeft;
        }
        public BodyPartMirror(string bodyTag, uint layer, Sprite partSprite, Sprite partSpriteLeft) : base(bodyTag, layer, partSprite)
        {
            this.layerLeft = layer;
            this.partSpriteLeft = partSpriteLeft;
        }
    }
}

#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(BodyPartsData))]
public class BodyPartsDataEditor : UnityEditor.Editor
{
    private readonly List<string> _spritesToRemove = new List<string>();
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var data = (BodyPartsData)target;

        serializedObject.Update();

        GUILayout.Space(10);
        var folderPath = UnityEditor.EditorGUILayout.TextField("Folder Path", "Assets/Mighty Heroes (Rogue) 2D Fantasy Characters Pack/Rogue");
        if (GUILayout.Button("Search Sprites on Folder"))
            SearchBodyParts(folderPath, data);
        if (GUILayout.Button("Clear Data"))
        {
            data.Hair.Clear();
            data.Face.Clear();
            data.Head.Clear();
            data.Torso.Clear();
            data.Pelvis.Clear();
            data.Shoulder.Clear();
            data.Elbow.Clear();
            data.Wrist.Clear();
            data.Leg.Clear();
            data.Boot.Clear();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void SearchBodyParts(string folderPath, BodyPartsData data)
    {
        var sprites = UnityEditor.AssetDatabase.FindAssets("t:Sprite", new[] { folderPath }).ToList();
        foreach (var sprite in sprites)
        {
            if (_spritesToRemove.Contains(sprite)) continue;
            var path = UnityEditor.AssetDatabase.GUIDToAssetPath(sprite);
            var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(path);
            var name = asset.name;
            if (path.Contains("Hair") || path.Contains("Hood"))
            {
                if(!data.Hair.Exists(x => x.bodyTag == name))
                    data.Hair.Add(new BodyPartsData.BodyPart(name, 13, asset));
            }
            else if (path.Contains("Face"))
            {
                if(!data.Face.Exists(x => x.bodyTag == name))
                    data.Face.Add(new BodyPartsData.BodyPart(name, 12, asset));
            }
            else if (path.Contains("Head"))
            {
                if(!data.Head.Exists(x => x.bodyTag == name))
                    data.Head.Add(new BodyPartsData.BodyPart(name, 11, asset));
            }
            else if (path.Contains("Torso"))
            {
                if(!data.Torso.Exists(x => x.bodyTag == name))
                    data.Torso.Add(new BodyPartsData.BodyPart(name, 7, asset));
            }
            else if (path.Contains("Pelvis"))
            {
                if(!data.Pelvis.Exists(x => x.bodyTag == name))
                    data.Pelvis.Add(new BodyPartsData.BodyPart(name, 6, asset));
            }
            else if (path.Contains("Shoulder"))
            {
                var assetOtherSide = OtherSideSprite(name, path, sprites, out var isLeft);

                name = name.Replace("_l_", "").Replace("_r_", "_");
                if(!data.Shoulder.Exists(x => x.bodyTag == name.Replace("_l_", "").Replace("_r_", "")))
                    data.Shoulder.Add(new BodyPartsData.BodyPartMirror(name, 5, 10, isLeft ? assetOtherSide : asset, !isLeft ? assetOtherSide : asset));
            }
            else if (path.Contains("Elbow"))
            {
                var assetOtherSide = OtherSideSprite(name, path, sprites, out var isLeft);

                name = name.Replace("_l_", "").Replace("_r_", "_");
                if(!data.Elbow.Exists(x => x.bodyTag == name.Replace("_l_", "").Replace("_r_", "")))
                    data.Elbow.Add(new BodyPartsData.BodyPartMirror(name, 4, 9, isLeft ? assetOtherSide : asset, !isLeft ? assetOtherSide : asset));
            }
            else if (path.Contains("Wrist"))
            {
                var assetOtherSide = OtherSideSprite(name, path, sprites, out var isLeft);

                name = name.Replace("_l_", "").Replace("_r_", "_");
                if(!data.Wrist.Exists(x => x.bodyTag == name.Replace("_l_", "").Replace("_r_", "")))
                    data.Wrist.Add(new BodyPartsData.BodyPartMirror(name, 3, 8, isLeft ? assetOtherSide : asset, !isLeft ? assetOtherSide : asset));
            }
            else if (path.Contains("Leg"))
            {
                var assetOtherSide = OtherSideSprite(name, path, sprites, out var isLeft);

                name = name.Replace("_l_", "").Replace("_r_", "_");
                if(!data.Leg.Exists(x => x.bodyTag == name.Replace("_l_", "").Replace("_r_", "")))
                    data.Leg.Add(new BodyPartsData.BodyPartMirror(name, 2, isLeft ? assetOtherSide : asset, !isLeft ? assetOtherSide : asset));
            }
            else if (path.Contains("Boot"))
            {
                var assetOtherSide = OtherSideSprite(name, path, sprites, out var isLeft);

                name = name.Replace("_l_", "").Replace("_r_", "_");
                if(!data.Boot.Exists(x => x.bodyTag == name))
                    data.Boot.Add(new BodyPartsData.BodyPartMirror(name, 1, isLeft ? assetOtherSide : asset, !isLeft ? assetOtherSide : asset));
            }
        }

        UnityEditor.EditorUtility.SetDirty(data);
    }

    private Sprite OtherSideSprite(string name, string path, List<string> sprites, out bool isLeft)
    {
        isLeft = name.Contains("_l_");
        var pathOS = path.Replace(isLeft ? "_l_" : "_r_", isLeft ? "_r_" : "_l_");
        var assetOtherSide = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(pathOS);
        _spritesToRemove.Add(sprites.Find(x => UnityEditor.AssetDatabase.AssetPathToGUID(pathOS) == x));
        return assetOtherSide;
    }
}
#endif