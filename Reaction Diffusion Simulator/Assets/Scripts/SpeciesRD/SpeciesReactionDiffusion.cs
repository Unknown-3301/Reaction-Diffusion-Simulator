using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeciesReactionDiffusion
{
    private ComputeShader shader;

    private RenderTexture abInput;
    private RenderTexture abOutput;

    private RenderTexture specieInput;
    private RenderTexture specieOutput;

    private RenderTexture colored;

    private int drawIndex;
    public int DrawSpecieIndex
    {
        get { return drawIndex; }
        set
        {
            if (value >= Species.Buffer.count)
                drawIndex = 0;
            else if (value < 0)
                drawIndex = Species.Buffer.count - 1;
            else
                drawIndex = value;
        }
    }

    public int Speed { get; set; }
    public float DrawRadius { get; set; }
    public int NumberOfColorBandsPerSpecie { get; }

    public bool IsMono { get; private set; }
    public ListComputeBuffer<ColorBandPiece> ColorsBand { get; private set; }
    public ListComputeBuffer<SimulationSettings> Species { get; private set; }

    public SpeciesReactionDiffusion(int width, int height, int speed, float drawRadius, int numberOfColorBandsPerSpecie, SimulationSettings simulation, ListComputeBuffer<ColorBandPiece> colorBands, ComputeShader shader)
    {
        this.shader = shader;

        DrawRadius = drawRadius;
        Speed = speed;
        NumberOfColorBandsPerSpecie = numberOfColorBandsPerSpecie;

        abInput = CreateABTexture(width, height);
        abOutput = CreateABTexture(width, height);

        specieInput = CreateSpecieTexture(width, height);
        specieOutput = CreateSpecieTexture(width, height);

        colored = CreateColoredTexture(width, height);

        ConvertToMono(simulation, colorBands);

        Clear();
    }

    public void ConvertToMono(SimulationSettings simulation, ListComputeBuffer<ColorBandPiece> colorBands)
    {
        if (ColorsBand != null)
        {
            ColorsBand.Destroy();
        }
        if (Species != null)
        {
            Species.Destroy();
        }

        Species = new ListComputeBuffer<SimulationSettings>(simulation, SimulationSettings.Size);
        ColorsBand = colorBands;

        IsMono = true;
    }
    public void ConvertToMulti(ListComputeBuffer<SimulationSettings> simulation, ListComputeBuffer<ColorBandPiece> colorBands)
    {
        Species.Destroy();

        Species = simulation;
        ColorsBand = colorBands;

        IsMono = false;
    }

    private RenderTexture CreateTexture(int width, int height, FilterMode filter, RenderTextureFormat textureFormat)
    {
        RenderTexture texture = new RenderTexture(width, height, 1);
        texture.filterMode = filter;
        texture.format = textureFormat;
        texture.enableRandomWrite = true;
        texture.Create();

        return texture;
    }
    private RenderTexture CreateABTexture(int width, int height) => CreateTexture(width, height, FilterMode.Bilinear, RenderTextureFormat.RGFloat);
    private RenderTexture CreateSpecieTexture(int width, int height) => CreateTexture(width, height, FilterMode.Point, RenderTextureFormat.RFloat);
    private RenderTexture CreateColoredTexture(int width, int height) => CreateTexture(width, height, FilterMode.Bilinear, RenderTextureFormat.Default);

    public void Clear()
    {
        int kernel = shader.FindKernel("Fill");

        shader.SetInt("TextureWidth", abInput.width);
        shader.SetInt("TextureHeight", abInput.height);

        shader.SetTexture(kernel, "Output", abInput);
        shader.SetTexture(kernel, "OutputSpeciesMap", specieInput);

        shader.Dispatch(kernel, Mathf.CeilToInt(abInput.width / 8f), Mathf.CeilToInt(abInput.height / 8f), 1);
    }
    public void ScaleDimensions(int newWidth, int newHeight)
    {
        abOutput = CreateABTexture(newWidth, newHeight);
        specieOutput = CreateSpecieTexture(newWidth, newHeight);
        colored = CreateColoredTexture(newWidth, newHeight);

        RenderTexture newInput = CreateABTexture(newWidth, newHeight);
        RenderTexture newSpecies = CreateSpecieTexture(newWidth, newHeight);

        Graphics.Blit(abInput, newInput);
        Graphics.Blit(specieInput, newSpecies);

        abInput = newInput;
        specieInput = newSpecies;
    }
    public void ChangeDimensions(int newWidth, int newHeight)
    {
        abOutput = CreateABTexture(newWidth, newHeight);
        specieOutput = CreateSpecieTexture(newWidth, newHeight);
        colored = CreateColoredTexture(newWidth, newHeight);

        RenderTexture newInput = CreateABTexture(newWidth, newHeight);
        RenderTexture newSpecies = CreateSpecieTexture(newWidth, newHeight);

        Vector4 inputRect = new Vector4();
        inputRect.z = abInput.width;
        inputRect.w = abInput.height;
        inputRect.x = (newWidth - abInput.width) / 2;
        inputRect.y = (newHeight - abInput.height) / 2;

        int kernel = shader.FindKernel("ChangeDimensions");

        shader.SetTexture(kernel, "Input", abInput);
        shader.SetTexture(kernel, "Output", newInput);
        shader.SetTexture(kernel, "InputSpeciesMap", specieInput);
        shader.SetTexture(kernel, "OutputSpeciesMap", newSpecies);

        shader.SetVector("InputRectToOutput", inputRect);

        shader.SetInt("TextureWidth", newWidth);
        shader.SetInt("TextureHeight", newHeight);

        shader.Dispatch(kernel, Mathf.CeilToInt(newWidth / 8f), Mathf.CeilToInt(newHeight / 8f), 1);

        abInput = newInput;
        specieInput = newSpecies;
    }
    public void ImportImage(Texture2D texture)
    {
        int kernel = shader.FindKernel("ImportTexture");

        shader.SetTexture(kernel, "Texture", texture);
        shader.SetTexture(kernel, "Output", abInput);
        shader.SetTexture(kernel, "OutputSpeciesMap", specieInput);

        shader.SetInt("DrawIndex", drawIndex);

        Vector4 inputRect = new Vector4();
        inputRect.z = texture.width;
        inputRect.w = texture.height;
        inputRect.x = (abInput.width - texture.width) / 2;
        inputRect.y = (abInput.height - texture.height) / 2;

        shader.SetVector("InputRectToOutput", inputRect);
        shader.SetInt("TextureWidth", abInput.width);
        shader.SetInt("TextureHeight", abInput.height);

        shader.Dispatch(kernel, Mathf.CeilToInt(abInput.width / 8f), Mathf.CeilToInt(abInput.height / 8f), 1);
    }
    public void DeleteSpecie(int index)
    {
        int kernel = shader.FindKernel("DeleteSpecie");

        shader.SetInt("TextureWidth", abInput.width);
        shader.SetInt("TextureHeight", abInput.height);

        shader.SetInt("DeleteIndex", index);

        shader.SetTexture(kernel, "OutputSpeciesMap", specieInput);

        shader.Dispatch(kernel, Mathf.CeilToInt(abInput.width / 8f), Mathf.CeilToInt(abInput.height / 8f), 1);
    }
    public void Draw(Vector2 relativeMousePosition)
    {
        int kernel = shader.FindKernel("Draw");

        shader.SetInt("TextureWidth", abInput.width);
        shader.SetInt("TextureHeight", abInput.height);

        shader.SetFloat("DrawRadius", DrawRadius);
        shader.SetVector("DrawPosition", relativeMousePosition);
        shader.SetInt("DrawIndex", drawIndex);

        shader.SetTexture(kernel, "Output", abInput);
        shader.SetTexture(kernel, "OutputSpeciesMap", specieInput);

        shader.Dispatch(kernel, Mathf.CeilToInt(abInput.width / 8f), Mathf.CeilToInt(abInput.height / 8f), 1);
    }
    public void Colorize(RenderTexture final)
    {
        int kernel = shader.FindKernel("Colorize");

        shader.SetInt("TextureWidth", abInput.width);
        shader.SetInt("TextureHeight", abInput.height);
        shader.SetInt("NumberOfColorsBandPerSpecie", NumberOfColorBandsPerSpecie);

        shader.SetBuffer(kernel, "ColorBands", ColorsBand.Buffer);

        shader.SetTexture(kernel, "Input", abInput);
        shader.SetTexture(kernel, "Final", colored);
        shader.SetTexture(kernel, "InputSpeciesMap", specieInput);

        shader.Dispatch(kernel, Mathf.CeilToInt(abInput.width / 8f), Mathf.CeilToInt(abInput.height / 8f), 1);

        Graphics.Blit(colored, final);
    }
    public void Simulate()
    {
        for (int i = 0; i < Speed; i++)
        {
            int kernel = shader.FindKernel("Simulate");

            shader.SetInt("TextureWidth", abInput.width);
            shader.SetInt("TextureHeight", abInput.height);
            shader.SetInt("NumberOfSpecies", Species.Buffer.count);

            shader.SetBuffer(kernel, "Species", Species.Buffer);

            shader.SetTexture(kernel, "Input", abInput);
            shader.SetTexture(kernel, "Output", abOutput);
            shader.SetTexture(kernel, "InputSpeciesMap", specieInput);
            shader.SetTexture(kernel, "OutputSpeciesMap", specieOutput);

            shader.Dispatch(kernel, Mathf.CeilToInt(abInput.width / 8f), Mathf.CeilToInt(abInput.height / 8f), 1);

            BlitAll();
        }
    }

    private void BlitAll()
    {
        int kernel = shader.FindKernel("BlitAll");

        shader.SetInt("TextureWidth", abInput.width);
        shader.SetInt("TextureHeight", abInput.height);

        shader.SetTexture(kernel, "Input", abOutput);
        shader.SetTexture(kernel, "Output", abInput);
        shader.SetTexture(kernel, "InputSpeciesMap", specieOutput);
        shader.SetTexture(kernel, "OutputSpeciesMap", specieInput);

        shader.Dispatch(kernel, Mathf.CeilToInt(abInput.width / 8f), Mathf.CeilToInt(abInput.height / 8f), 1);
    }

    public void Destroy()
    {
        abInput.Release();
        abOutput.Release();
        specieInput.Release();
        specieOutput.Release();
        colored.Release();

        ColorsBand.Destroy();
        Species.Destroy();
    }
}
