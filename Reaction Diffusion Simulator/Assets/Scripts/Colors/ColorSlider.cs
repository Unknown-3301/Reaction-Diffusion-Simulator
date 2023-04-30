using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ColorSlider : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Slider slider;

    private float bias;

    public int Index { get; set; }
    public ColorBandPiece ColorBand
    {
        get => new ColorBandPiece()
        {
            PositionInLine = slider.value,
            R = image.color.r,
            G = image.color.g,
            B = image.color.b,
            A = image.color.a,
            Bias = bias
        };
    }
    
    public void SetColorBandSilent(ColorBandPiece newBand)
    {
        slider.SetValueWithoutNotify(newBand.PositionInLine);
        image.color = new Color(newBand.R, newBand.G, newBand.B, newBand.A);
        bias = newBand.Bias;
    }
    public void SetColorWithoutPosition(ColorBandPiece newBand)
    {
        image.color = new Color(newBand.R, newBand.G, newBand.B, newBand.A);
        bias = newBand.Bias;
    }
    public void SetColorBandNonSilent(ColorBandPiece newBand)
    {
        slider.value = newBand.PositionInLine;
        image.color = new Color(newBand.R, newBand.G, newBand.B, newBand.A);
        bias = newBand.Bias;
    }

    public void OnHandleClick() => ColorEditor.Edit(Index, ColorBand);
}
