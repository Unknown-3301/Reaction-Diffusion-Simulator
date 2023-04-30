using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ColorEditor : MonoBehaviour
{
    private static ColorEditor manager;

    [SerializeField] private InputSlider h, s, v, bias;
    [SerializeField] private Material saturationMat, valueMat;
    [SerializeField] private Image viewImage;

    public static bool Active { get => manager.gameObject.activeInHierarchy; set => manager.gameObject.SetActive(value); }
    public static int Index { get; private set; }
    public static ColorBandPiece ColorBand { get; private set; }
    public static Action OnChange { get; set; }

    private void Awake()
    {
        manager = this;
        gameObject.SetActive(false);
    }
    public void OnValueChange()
    {
        saturationMat.SetColor("MainColor", Color.HSVToRGB(h.Value, 1, 1));
        valueMat.SetColor("MainColor", Color.HSVToRGB(h.Value, s.Value, 1));

        Color color = Color.HSVToRGB(h.Value, s.Value, v.Value);

        ColorBand = new ColorBandPiece()
        { 
            R = color.r, 
            G = color.g, 
            B = color.b, 
            A = color.a,
            Bias = bias.Value, 
            PositionInLine = ColorBand.PositionInLine 
        };

        viewImage.color = color;

        if (OnChange != null)
        {
            OnChange();
        }
    }

    public static void Edit(int index, ColorBandPiece band)
    {
        Active = true;

        ColorBand = band;
        Index = index;

        Color rgb = new Color(band.R, band.G, band.B, band.A);

        float hue, staturation, value;
        Color.RGBToHSV(rgb, out hue, out staturation, out value);

        manager.h.SetValueSilent(hue);
        manager.s.SetValueSilent(staturation);
        manager.v.SetValueSilent(value);
        manager.bias.SetValueSilent(band.Bias);

        manager.saturationMat.SetColor("MainColor", Color.HSVToRGB(manager.h.Value, 1, 1));
        manager.valueMat.SetColor("MainColor", Color.HSVToRGB(manager.h.Value, manager.s.Value, 1));

        manager.viewImage.color = rgb;
    }
}
