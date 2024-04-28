using System.Collections.Generic;
using System.Linq;
using BGS.Data;
using UnityEngine;
#if UNITY_EDITOR
    namespace BGS.Editor
    {
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
                var folderPath = UnityEditor.EditorGUILayout.TextField("Folder Path", "Assets/Packages/Mighty Heroes (Rogue) 2D Fantasy Characters Pack/Rogue");
                if (GUILayout.Button("Search Sprites on Folder"))
                    SearchBodyParts(folderPath, data);
                if (GUILayout.Button("Clear Data"))
                {
                    data.hair.Clear();
                    data.face.Clear();
                    data.head.Clear();
                    data.torso.Clear();
                    data.pelvis.Clear();
                    data.shoulder.Clear();
                    data.elbow.Clear();
                    data.wrist.Clear();
                    data.leg.Clear();
                    data.boot.Clear();
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
                        if(!data.hair.Exists(x => x.bodyTag == name))
                            data.hair.Add(new BodyPartsData.BodyPart(name, 13, asset, -17.6f));
                    }
                    else if (path.Contains("Face"))
                    {
                        if(!data.face.Exists(x => x.bodyTag == name))
                            data.face.Add(new BodyPartsData.BodyPart(name, 12, asset, 1.7f));
                    }
                    else if (path.Contains("Head"))
                    {
                        if(!data.head.Exists(x => x.bodyTag == name))
                            data.head.Add(new BodyPartsData.BodyPart(name, 11, asset, -10.4f));
                    }
                    else if (path.Contains("Torso"))
                    {
                        if(!data.torso.Exists(x => x.bodyTag == name))
                            data.torso.Add(new BodyPartsData.BodyPart(name, 7, asset, 31));
                    }
                    else if (path.Contains("Pelvis"))
                    {
                        if(!data.pelvis.Exists(x => x.bodyTag == name))
                            data.pelvis.Add(new BodyPartsData.BodyPart(name, 6, asset, 18.5f));
                    }
                    else if (path.Contains("Shoulder"))
                    {
                        var assetOtherSide = OtherSideSprite(name, path, sprites, out var isLeft);

                        name = name.Replace("_l_", "").Replace("_r_", "_");
                        if(!data.shoulder.Exists(x => x.bodyTag == name.Replace("_l_", "").Replace("_r_", "")))
                            data.shoulder.Add(new BodyPartsData.BodyPartPairs(name, 5, 10, isLeft ? assetOtherSide : asset, !isLeft ? assetOtherSide : asset, 20.7f));
                    }
                    else if (path.Contains("Elbow"))
                    {
                        var assetOtherSide = OtherSideSprite(name, path, sprites, out var isLeft);

                        name = name.Replace("_l_", "").Replace("_r_", "_");
                        if(!data.elbow.Exists(x => x.bodyTag == name.Replace("_l_", "").Replace("_r_", "")))
                            data.elbow.Add(new BodyPartsData.BodyPartPairs(name, 4, 9, isLeft ? assetOtherSide : asset, !isLeft ? assetOtherSide : asset, 16.3f));
                    }
                    else if (path.Contains("Wrist"))
                    {
                        var assetOtherSide = OtherSideSprite(name, path, sprites, out var isLeft);

                        name = name.Replace("_l_", "").Replace("_r_", "_");
                        if(!data.wrist.Exists(x => x.bodyTag == name.Replace("_l_", "").Replace("_r_", "")))
                            data.wrist.Add(new BodyPartsData.BodyPartPairs(name, 3, 8, isLeft ? assetOtherSide : asset, !isLeft ? assetOtherSide : asset, 23.2f));
                    }
                    else if (path.Contains("Leg"))
                    {
                        var assetOtherSide = OtherSideSprite(name, path, sprites, out var isLeft);

                        name = name.Replace("_l_", "").Replace("_r_", "_");
                        if(!data.leg.Exists(x => x.bodyTag == name.Replace("_l_", "").Replace("_r_", "")))
                            data.leg.Add(new BodyPartsData.BodyPartPairs(name, 2, isLeft ? assetOtherSide : asset, !isLeft ? assetOtherSide : asset, 51.4f));
                    }
                    else if (path.Contains("Boot"))
                    {
                        var assetOtherSide = OtherSideSprite(name, path, sprites, out var isLeft);

                        name = name.Replace("_l_", "").Replace("_r_", "_");
                        if(!data.boot.Exists(x => x.bodyTag == name))
                            data.boot.Add(new BodyPartsData.BodyPartPairs(name, 1, isLeft ? assetOtherSide : asset, !isLeft ? assetOtherSide : asset, 47.1f));
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
    }
#endif