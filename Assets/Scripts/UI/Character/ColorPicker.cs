using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace BGS.UI.Character
{
    /// <summary>
    /// This class represents a color picker UI component.
    /// </summary>
    public class ColorPicker : MonoBehaviour
    {
        #region Variables
        
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
        private ColorBar hueBar;
        [SerializeField]
        [Tooltip("The saturation bar of the color picker.")]
        private ColorBar saturationBar;
        [SerializeField]
        [Tooltip("The value bar of the color picker.")]
        private ColorBar valueBar;
        
        [Tooltip("The event that will be triggered when the color changes.")]
        internal Action<Color, int> ColorChanged;

        [Tooltip("The hue, saturation, and value of the selected color.")]
        private float _h, _s, _v;
        [Tooltip("The index of the selected color.")]
        private int _colorIndex;
        
        #endregion

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
            this._colorIndex = colorIndex;
            pickerColor = col;
            UpdateSlidersColor();
        }
        
        /// <summary>
        /// This method updates the selected color based on the sliders' values.
        /// </summary>
        public void UpdateColor()
        {
            pickerColor = Color.HSVToRGB(hueBar.value, saturationBar.value, valueBar.value);
            Color.RGBToHSV(pickerColor, out _h, out _s, out _v);

            hueBar.barHandle.color = pickerColor;
            saturationBar.barHandle.color = pickerColor;
            valueBar.barHandle.color = pickerColor;

            saturationBar.barBackground.color = Color.HSVToRGB(_h, 1, _v);
            valueBar.barBackground.color = Color.HSVToRGB(_h, _s, 1);

            UpdateGraphic();
        }
        
        #endregion
        
        #region Private Methods
        
        /// <summary>
        /// This method updates the color of the sliders.
        /// </summary>
        private void UpdateSlidersColor()
        {
            Color.RGBToHSV(pickerColor, out _h, out _s, out _v);

            hueBar.barSlider.onValueChanged.AddListener(delegate { UpdateColor(); });
            saturationBar.barSlider.onValueChanged.AddListener(delegate { UpdateColor(); });
            valueBar.barSlider.onValueChanged.AddListener(delegate { UpdateColor(); });

            hueBar.value = _h;
            saturationBar.value = _s;
            valueBar.value = _v;

            graphicTarget.color = pickerColor;
        }
        
        /// <summary>
        /// This method updates the graphic target with the selected color.
        /// </summary>
        private void UpdateGraphic()
        {
            graphicTarget.color = pickerColor;
            ColorChanged?.Invoke(pickerColor, _colorIndex);
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