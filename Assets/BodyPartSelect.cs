using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodyPartSelect : MonoBehaviour
{
    [SerializeField] private uint id;
    [SerializeField] private Image icon;
    [SerializeField] private Button button;

    public void SetUp(uint id, Sprite icon, Action onClick)
    {
        this.id = id;
        this.icon.sprite = icon;
        button.onClick.AddListener(() => onClick());
    }
}
