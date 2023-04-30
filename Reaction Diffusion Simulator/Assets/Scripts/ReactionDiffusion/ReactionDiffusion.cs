using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ReactionDiffusion : MonoBehaviour
{
    [SerializeField] private ComputeShader ppShader;
    [SerializeField] private ComputeShader shader;

    [SerializeField] private RectTransform editorTransform;
    [SerializeField] private GameObject fullScreenHolder;
    [SerializeField] private GameObject fullScreenCanvas;

    [SerializeField] private FSP_Toggle fsp; 

    public static bool FullScreen { get; private set; }

    private RawImage renderImage;
    private RectTransform rectTransform;

    private SpeciesReactionDiffusion rd;

    private RenderTexture colored;
    private RenderTexture final;

    private Rect renderRect;
    private int qualityLevel = 1;


    public RenderTexture Image { get; private set; }
    public float DrawRadius { get => rd.DrawRadius; set => rd.DrawRadius = value; }
    public int DrawSpecieIndex { get => rd.DrawSpecieIndex; set => rd.DrawSpecieIndex = value; }
    public int Speed { get => rd.Speed; set => rd.Speed = value; }
    public bool Paused { get; set; }

    private int shaderWidth { get => (int)(renderRect.width / qualityLevel); }
    private int shaderHeight { get => (int)(renderRect.height / qualityLevel); }
    

    private void Start()
    {
        renderImage = GetComponent<RawImage>();
        rectTransform = GetComponent<RectTransform>();

        PostProcessingEffects.PostProcessingShader = ppShader;

        CreateTextures();

        SpecieManager.OnAddSpecie = OnAddSpecie;
        SpecieManager.OnSelectSpecie = OnSelectSpecie;
        SpecieManager.OnDeleteSpecie = OnDeleteSpecie;
        SpecieManager.OnManagerDisable = OnManagerDisable;
    }

    private RenderTexture CreateTexture2D(int width, int height)
    {
        RenderTexture texture = new RenderTexture(width, height, 1);
        texture.enableRandomWrite = true;
        texture.Create();
        return texture;
    }
    private void CreateTextures()
    {
        renderRect = GameManager.RectTransformToScreenSpace(rectTransform);

        rd = new SpeciesReactionDiffusion(shaderWidth, shaderHeight, 5, 10, ColorsBandManager.NumberOfBands, SimulationSettingsManager.Settings, ColorsBandManager.ColorBandsBuffer, shader);

        colored = CreateTexture2D(shaderWidth, shaderHeight);
        final = CreateTexture2D(shaderWidth, shaderHeight);
    }

    private void Update()
    {
        CheckDimensions();

        if (!FullScreen)
        {
            Paused = false;
        }
        
        if (!VisualSL.Active && !SimulationSL.Active)
        {
            if (!SpecieManager.IsInputOnFocused)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    ChangeFullScreen(!FullScreen);
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    ChangeFullScreen(false);
                }
            }

            if (Input.GetMouseButton(0))
            {
                Vector2 pos = (Input.mousePosition - new Vector3(renderRect.x, renderRect.y)) / qualityLevel;

                bool drawable = renderRect.Contains(Input.mousePosition);

                if (FullScreen)
                {
                    if (fsp.IsContainingMouse(Input.mousePosition))
                    {
                        drawable = false;
                    }
                }
                else
                {
                    if (ColorEditor.Active)
                    {
                        Rect rect = GameManager.RectTransformToScreenSpace(editorTransform);
                        if (rect.Contains(Input.mousePosition))
                        {
                            drawable = false;
                        }
                    }
                }

                if (drawable)
                    Draw((int)pos.x, (int)pos.y);
            }
        }
        

        if (renderImage.color == Color.black)
        {
            renderImage.color = Color.white;
        }

        if (!Paused)
        {
            if (rd.IsMono)
            {
                rd.Species.SetData(0, SimulationSettingsManager.Settings);
            }

            rd.Simulate();
        }

        rd.Colorize(colored);

        BloomSettings settings = BloomManager.Settings;
        if (settings.Enabled)
        {
            PostProcessingEffects.Bloom(colored, final, settings.BloomThreshold, settings.BloomBrightness, settings.BlurScale, (int)settings.BlurSamples, settings.BlurSD);
            renderImage.texture = final;
            Image = final;
        }
        else
        {
            renderImage.texture = colored;
            Image = colored;
        }
    }
    private void CheckDimensions()
    {
        Rect newRect = GameManager.RectTransformToScreenSpace(rectTransform);

        if (newRect.x == renderRect.x && newRect.y == renderRect.y && newRect.width == renderRect.width && newRect.height == renderRect.height)
            return;

        renderRect = newRect;

        rd.ChangeDimensions(shaderWidth, shaderHeight);

        colored = CreateTexture2D((int)newRect.width, (int)newRect.height);
        final = CreateTexture2D((int)newRect.width, (int)newRect.height);
    }

    public void DrawCenter() => Draw(shaderWidth / 2, shaderHeight / 2);
    public void importImage(Texture2D texture) => rd.ImportImage(texture);
    public void Draw(int posX, int posY) => rd.Draw(new Vector2(posX, posY));
    private void OnSelectSpecie(int index) => rd.DrawSpecieIndex = index;
    private void OnAddSpecie()
    {
        if (SpecieManager.NumberOfSpecies == 1)
        {
            rd.ConvertToMulti(SpecieManager.SimulationBuffer, SpecieManager.ColorsBuffer);
        }
    }
    private void OnDeleteSpecie(int index) => rd.DeleteSpecie(index);
    private void OnManagerDisable() => rd.ConvertToMono(SimulationSettingsManager.Settings, ColorsBandManager.ColorBandsBuffer);

    public void ChangeDrawRadius(string numText) => rd.DrawRadius = (int)GameManager.StringToFloat(numText);
    public void Clear() => rd.Clear();
    public void ChangeSpeed(string numText)
    {
        rd.Speed = (int)GameManager.StringToFloat(numText);
        Paused = false;
    }
    public void ChangeQuality(Int32 newQual)
    {
        qualityLevel = newQual + 1;
        rd.ScaleDimensions(shaderWidth, shaderHeight);
    }

    public void ChangeFullScreen(bool fullSceen)
    {
        if (fullSceen == FullScreen)
            return;

        fullScreenHolder.SetActive(fullSceen);

        rectTransform = (fullSceen ? fullScreenCanvas : gameObject).GetComponent<RectTransform>();
        renderImage = (fullSceen ? fullScreenCanvas : gameObject).GetComponent<RawImage>();

        FullScreen = fullSceen;
    }

    private void OnDestroy()
    {
        colored.Release();
        final.Release();

        rd.Destroy();
    }
}
