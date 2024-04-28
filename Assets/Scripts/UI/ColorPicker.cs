using System;
using UnityEngine;
using UnityEngine.UI;

namespace BGS.UI
{
    /// <summary>
    /// This class represents a color picker UI component.
    /// </summary>
    public class ColorPicker : MonoBehaviour
    {
        [Header("Color Picker")]
        [SerializeField]
        [Tooltip("The initial color of the color picker.")]
        private Color pickerColor = new Color(.565f, .565f, .565f);
        [SerializeField]
        [Tooltip("The graphic target that will be updated with the selected color.")]
        private Graphic graphicTarget;

        [Space]
        [Header("Color Bars")]
        [SerializeField]
        [Tooltip("The hue bar of the color picker.")]
        private ColorBar HUEBar;
        [SerializeField]
        [Tooltip("The saturation bar of the color picker.")]
        private ColorBar SaturationBar;
        [SerializeField]
        [Tooltip("The value bar of the color picker.")]
        private ColorBar ValueBar;
        
        [Tooltip("The event that will be triggered when the color changes.")]
        internal Action<Color, int> onColorChanged;

        [Tooltip("The hue, saturation, and value of the selected color.")]
        private float h, s, v;
        [Tooltip("The index of the selected color.")]
        private int colorIndex;

        #region Unity Callbacks

        /// <summary>
        /// This method is called when the color picker is enabled to update the sliders color.
        /// </summary>
        private void OnEnable()
        {
            UpdateSlidersColor();
        }
        
        #endregion

        #region Public Methods
        
        /// <summary>
        /// This method sets up the color picker with a specific color.
        /// </summary>
        /// <param name="colorIndex">The index of the color.</param>
        /// <param name="col">The color to set up the color picker with.</param>
        public void SetUpColorPicker(int colorIndex, Color col)
        {
            this.colorIndex = colorIndex;
            pickerColor = col;
            UpdateSlidersColor();
        }
        
        /// <summary>
        /// This method updates the selected color based on the sliders' values.
        /// </summary>
        public void UpdateColor()
        {
            pickerColor = Color.HSVToRGB(HUEBar.value, SaturationBar.value, ValueBar.value);
            Color.RGBToHSV(pickerColor, out h, out s, out v);

            HUEBar.barHandle.color = pickerColor;
            SaturationBar.barHandle.color = pickerColor;
            ValueBar.barHandle.color = pickerColor;

            SaturationBar.barBackground.color = Color.HSVToRGB(h, 1, v);
            ValueBar.barBackground.color = Color.HSVToRGB(h, s, 1);

            UpdateGraphic();
        }
        
        #endregion
        
        #region Private Methods
        
        /// <summary>
        /// This method updates the color of the sliders.
        /// </summary>
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
        
        /// <summary>
        /// This method updates the graphic target with the selected color.
        /// </summary>
        private void UpdateGraphic()
        {
            graphicTarget.color = pickerColor;
            onColorChanged?.Invoke(pickerColor, colorIndex);
        }
        
        #endregion
    }

    /// <summary>
    /// This class represents a color bar in the color picker.
    /// </summary>
    [System.Serializable]
    public class ColorBar
    {
        [Tooltip("The slider of the color bar.")]
        public Slider barSlider;
        [Tooltip("The handle of the color bar.")]
        public Image barHandle;
        [Tooltip("The background of the color bar.")]
        public Image barBackground;

        // The value of the color bar.
        public float value
        {
            get => barSlider.value;
            set => barSlider.value = value;
        }
    }
}