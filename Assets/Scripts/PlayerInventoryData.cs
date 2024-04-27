using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Inventory Data", menuName = "BGS/Player Inventory Data")]
public class PlayerInventoryData : ScriptableObject
{
    public int money;

    public List<BodyPartItem> bodyParts = new List<BodyPartItem>();

    private void OnValidate()
    {
#if UNITY_EDITOR
        // Remove the possibility of having the same body part twice modifying the list in unity editor
        for (int i = 0; i < bodyParts.Count; i++)
        {
            for (int j = i + 1; j < bodyParts.Count; j++)
            {
                if (bodyParts[i].type == bodyParts[j].type && bodyParts[i].id == bodyParts[j].id)
                {
                    bodyParts.RemoveAt(j);
                    j--;
                }
            }
        }
#endif
    }
}

[Serializable]
public class BodyPartItem
{
    public BodyPartType type;
    public int id;
}