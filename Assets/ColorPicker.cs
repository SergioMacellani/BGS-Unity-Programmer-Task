using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ColorPicker : MonoBehaviour
{
    public Color pickerColor = new Color(.565f,.565f,.565f);
    public Graphic graphicTarget;

    public ColorBar HUEBar;
    public ColorBar SaturationBar;
    public ColorBar ValueBar;

    public Action<Color, int> onColorChanged;

    private float h, s, v;
    private int colorIndex;

    private void OnEnable()
    {
        UpdateSlidersColor();
    }

    private void UpdateSlidersColor()
    {
        Color.RGBToHSV(pickerColor, out h, out s, out v);

        HUEBar.barSlider.onValueChanged.AddListener(delegate { UpdateColor(); });
        SaturationBar.barSlider.onValueChanged.AddListener(delegate { UpdateColor(); });
        ValueBar.barSlider.onValueChanged.AddListener(delegate { UpdateColor(); });

        HUEBar.value = h;
        SaturationBar.value = s;
        ValueBar.value = v;

        graphicTarget.color = pickerColor;
    }

    public void UpdateColor()
    {
        pickerColor = Color.HSVToRGB(HUEBar.value,SaturationBar.value,ValueBar.value);
        Color.RGBToHSV(pickerColor, out h, out s, out v);

        HUEBar.barHandle.color = pickerColor;
        SaturationBar.barHandle.color = pickerColor;
        ValueBar.barHandle.color = pickerColor;

        SaturationBar.barBackground.color = Color.HSVToRGB(h,1,v);
        ValueBar.barBackground.color = Color.HSVToRGB(h,s,1);

        UpdateGraphic();
    }

    private void UpdateGraphic()
    {
        graphicTarget.color = pickerColor;
        onColorChanged?.Invoke(pickerColor, colorIndex);
    }

    public void OpenColorPicker(int colorIndex, Color col)
    {
        gameObject.SetActive(true);

        this.colorIndex = colorIndex;
        pickerColor = col;
        UpdateSlidersColor();
    }
}

[System.Serializable]
public class ColorBar
{
    public Slider barSlider;
    public Image barHandle;
    public Image barBackground;

    public float value
    {
        get => barSlider.value;
        set => barSlider.value = value;
    }
}