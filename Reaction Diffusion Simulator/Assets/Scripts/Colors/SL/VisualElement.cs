using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class VisualElement : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI nameUGUI;

    public string Name { get => nameUGUI.text;  set => nameUGUI.text = value; }
    public VisualSettings Settings { get; private set; }

    private ListComputeBuffer<ColorBandPiece> colorBuffer;

    public void Init(VisualSettings settings)
    {
        Settings = settings;
        Name = settings.Name;

        colorBuffer = new ListComputeBuffer<ColorBandPiece>(settings.ColorBands, ColorBandPiece.Size);

        image.material = Instantiate(image.material);
        image.material.SetBuffer("ColorBands", colorBuffer.Buffer);
        image.material.SetInt("NumberOfBands", colorBuffer.Buffer.count);
    }

    public void Select() => VisualSL.Load(Settings);
    public void Delete()
    {
        GameManager.Delete(VisualSL.path + "/" + Name + VisualSL.extention);
        Destroy(gameObject);
    }

    private void OnDestroy() => colorBuffer.Buffer.Release();
}
