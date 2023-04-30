using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ColorsBandManager : MonoBehaviour
{
    private static ColorsBandManager manager;

    private ColorSlider[] sliders;
    private Image image;
    private bool changedThisFrame;

    public static int NumberOfBands { get; private set; }
    public static ListComputeBuffer<ColorBandPiece> ColorBandsBuffer { get; private set; }
    public static List<ColorBandPiece> ColorBands { get; private set; }
    public static Action OnChange { get; set; }

    private void Awake() => manager = this;
    private void Update() => changedThisFrame = false;
    private void Start()
    {
        NumberOfBands = manager.transform.childCount;
        image = GetComponent<Image>();

        InitBands();

        ColorEditor.OnChange = OnEditorChange;
    }

    private void UpDateBufferAndMaterial()
    {
        ColorBandsBuffer.UpdateBuffer();
        image.material.SetBuffer("ColorBands", ColorBandsBuffer.Buffer);
        image.material.SetInt("NumberOfBands", ColorBandsBuffer.Buffer.count);
    }
    private void InitBands()
    {
        ColorBands = new List<ColorBandPiece>(NumberOfBands);
        sliders = new ColorSlider[NumberOfBands];

        for (int i = 0; i < NumberOfBands; i++)
        {
            sliders[i] = transform.GetChild(i).GetComponent<ColorSlider>();
            sliders[i].Index = i;

            ColorBands.Add(sliders[i].ColorBand);
        }

        ColorBands.Sort();


        ColorBandsBuffer = new ListComputeBuffer<ColorBandPiece>(ColorBands, ColorBandPiece.Size);

        UpDateBufferAndMaterial();
    }

    public static void SetColorBandsSilent(List<ColorBandPiece> colors)
    {
        for (int i = 0; i < manager.sliders.Length; i++)
        {
            manager.sliders[i].SetColorBandSilent(colors[i]);
            ColorBands[i] = colors[i];
        }

        manager.UpDateBufferAndMaterial();
    }
    public static void SetColorBandsNonSilent(List<ColorBandPiece> colors)
    {
        for (int i = 0; i < manager.sliders.Length; i++)
        {
            ColorBands[i] = colors[i];
            manager.sliders[i].SetColorBandSilent(colors[i]);
        }

        manager.OnValueChange();
    }

    public void OnEditorChange()
    {
        sliders[ColorEditor.Index].SetColorWithoutPosition(ColorEditor.ColorBand);

        for (int i = 0; i < sliders.Length; i++)
        {
            ColorBands[i] = sliders[i].ColorBand;
        }
        ColorBands.Sort();

        ColorBandsBuffer.UpdateBuffer();

        manager.OnValueChange();
    }
    public void OnValueChange()
    {
        if (changedThisFrame)
            return;

        UpDateBufferAndMaterial();

        if (OnChange != null)
        {
            OnChange();
        }

        changedThisFrame = true;
    }
    public void OnSliderChange()
    {
        for (int i = 0; i < sliders.Length; i++)
        {
            ColorBands[i] = sliders[i].ColorBand;
        }
        ColorBands.Sort();

        OnValueChange();
    }

    private void OnDestroy()
    {
        ColorBandsBuffer.Buffer.Release();
    }
}
