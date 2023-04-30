using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SpecieElement : MonoBehaviour
{
    [SerializeField] private GameObject deleteButton;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color normalColor;

    public SimulationSettings SimulationSettings { get; set; }
    public List<ColorBandPiece> ColorBands { get; private set; }

    public int Index { get; set; }
    public bool Deletable { get => deleteButton.activeInHierarchy; set => deleteButton.SetActive(value); }
    public bool Selected { get => nameText.color == selectedColor; set => nameText.color = value ? selectedColor : normalColor; }
    public string Name { get { return nameText.text; } set { nameText.text = value; } }

    private bool matrixInit;
    private ListComputeBuffer<ColorBandPiece> colorBandsBuffer;

    public void Select() => SpecieManager.SelectSpecie(this);
    public void Delete() => SpecieManager.DeleteSpecie(Index);

    public void SetColors(List<ColorBandPiece> bands)
    {
        if (!matrixInit)
        {
            Init(bands);
        }
        else
        {
            UpdataData(bands);
        }

        icon.material.SetBuffer("ColorBands", colorBandsBuffer.Buffer);
        icon.material.SetInt("NumberOfBands", colorBandsBuffer.Buffer.count);
    }
    private void Init(List<ColorBandPiece> bands)
    {
        ColorBands = new List<ColorBandPiece>(bands);
        colorBandsBuffer = new ListComputeBuffer<ColorBandPiece>(ColorBands, ColorBandPiece.Size);

        icon.material = Instantiate(icon.material);
        matrixInit = true;
    }
    public void UpdataData(List<ColorBandPiece> bands)
    {
        for (int i = 0; i < bands.Count; i++)
        {
            ColorBands[i] = bands[i];
        }

        colorBandsBuffer.UpdateBuffer();
    }

    private void OnDestroy()
    {
        colorBandsBuffer.Buffer.Release();
    }
}
