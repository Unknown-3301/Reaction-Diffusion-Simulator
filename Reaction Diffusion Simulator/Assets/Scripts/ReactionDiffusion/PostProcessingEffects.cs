using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PostProcessingEffects
{
    public static ComputeShader PostProcessingShader { get; set; }

    /// <summary>
    /// Add Glow Effect for the bright colors to texture.
    /// </summary>
    /// <param name="texture">The texture to add glow to.</param>
    /// <param name="brightnessThreshhold">The brightness threshhold to consider a color bright. Range[0, 1]</param>
    /// <param name="bloomStength">How strong is the glow effect. Range[0, infinity]</param>
    /// <param name="blurScale">The higher the value the faster the blurr but the worse the quality. Range[0, infinity]</param>
    /// <param name="blurSize">How big is the blur. The higher the value the more the glow spread.</param>
    /// <param name="blurStandardDeviation"></param>
    /// <returns></returns>
    public static void Bloom(RenderTexture texture, RenderTexture outputTexture, float brightnessThreshhold, float bloomStength, int blurScale, int blurSize, float blurStandardDeviation)
    {
        RenderTexture blurTexture = CreateTempTexture(texture);

        ThreshHold(blurTexture, brightnessThreshhold);
        Blur(blurTexture, blurScale, blurSize, blurStandardDeviation);

        int kernel = PostProcessingShader.FindKernel("Bloom");
        SetShaderVariables2Texture(texture, outputTexture, kernel);

        PostProcessingShader.SetTexture(kernel, "BlurTexture", blurTexture);
        PostProcessingShader.SetFloat("BloomStength", bloomStength);

        PostProcessingShader.Dispatch(kernel, Mathf.CeilToInt(texture.width / 8f), Mathf.CeilToInt(texture.height / 8f), 1);

        RenderTexture.ReleaseTemporary(blurTexture);
    }
    /// <summary>
    /// Apply Blur effect on "texture"
    /// </summary>
    /// <param name="texture"></param>
    /// <param name="blurScale"></param>
    /// <param name="blurSize"></param>
    /// <param name="blurStandardDeviation"></param>
    public static void Blur(RenderTexture texture, int blurScale, int blurSize, float blurStandardDeviation)
    {
        if (blurScale > 1)
        {
            RenderTexture input = RenderTexture.GetTemporary(texture.width / blurScale, texture.height / blurScale);
            input.enableRandomWrite = true;
            input.Create();

            Graphics.Blit(texture, input);

            RenderTexture output = RenderTexture.GetTemporary(texture.width / blurScale, texture.height / blurScale);
            output.enableRandomWrite = true;
            output.Create();

            // HORIZONTAL
            int kernel = PostProcessingShader.FindKernel("HorizontalBlur");
            SetShaderVariables2Texture(input, output, kernel);

            PostProcessingShader.SetFloat("BlurSize", blurSize);
            PostProcessingShader.SetFloat("BlurStandardDeviation", blurStandardDeviation);

            PostProcessingShader.Dispatch(kernel, Mathf.CeilToInt(output.width / 8f), Mathf.CeilToInt(output.height / 8f), 1);
            //

            Graphics.Blit(output, input);

            // VERTICAL
            kernel = PostProcessingShader.FindKernel("VerticalBlur");
            SetShaderVariables2Texture(input, output, kernel);

            PostProcessingShader.SetFloat("BlurSize", blurSize);
            PostProcessingShader.SetFloat("BlurStandardDeviation", blurStandardDeviation);

            PostProcessingShader.Dispatch(kernel, Mathf.CeilToInt(output.width / 8f), Mathf.CeilToInt(output.height / 8f), 1);
            //

            Graphics.Blit(output, texture);

            RenderTexture.ReleaseTemporary(input);
            RenderTexture.ReleaseTemporary(output);
        }
        else
        {
            RenderTexture input = CreateTempTexture(texture);

            // HORIZONTAL
            int kernel = PostProcessingShader.FindKernel("HorizontalBlur");
            SetShaderVariables2Texture(input, texture, kernel);

            PostProcessingShader.SetFloat("BlurSize", blurSize);
            PostProcessingShader.SetFloat("BlurStandardDeviation", blurStandardDeviation);

            PostProcessingShader.Dispatch(kernel, Mathf.CeilToInt(texture.width / 8f), Mathf.CeilToInt(texture.height / 8f), 1);

            //
            Graphics.Blit(texture, input);

            // VERTICAL
            kernel = PostProcessingShader.FindKernel("VerticalBlur");
            SetShaderVariables2Texture(input, texture, kernel);

            PostProcessingShader.SetFloat("BlurSize", blurSize);
            PostProcessingShader.SetFloat("BlurStandardDeviation", blurStandardDeviation);

            PostProcessingShader.Dispatch(kernel, Mathf.CeilToInt(texture.width / 8f), Mathf.CeilToInt(texture.height / 8f), 1);
            //

            RenderTexture.ReleaseTemporary(input);
        }
    }
    /// <summary>
    /// Apply threshhold effect on "texture"
    /// </summary>
    /// <param name="texture"></param>
    /// <param name="brightnessThreshhold"></param>
    public static void ThreshHold(RenderTexture texture, float brightnessThreshhold)
    {
        int kernel = PostProcessingShader.FindKernel("ThreshHold");
        SetShaderVariables1Texture(texture, kernel);

        PostProcessingShader.SetFloat("BrightnessThreshhold", brightnessThreshhold);

        PostProcessingShader.Dispatch(kernel, Mathf.CeilToInt(texture.width / 8f), Mathf.CeilToInt(texture.height / 8f), 1);
    }

    public static RenderTexture CreateTempTexture(RenderTexture copyFrom)
    {
        RenderTexture t = RenderTexture.GetTemporary(copyFrom.width, copyFrom.height);
        t.enableRandomWrite = true;
        t.Create();

        Graphics.Blit(copyFrom, t);

        return t;
    }
        
    private static void SetShaderVariables2Texture(RenderTexture texture1, RenderTexture texture2, int kernel)
    {
        PostProcessingShader.SetTexture(kernel, "InputTexture", texture1);
        PostProcessingShader.SetTexture(kernel, "OutputTexture", texture2);

        PostProcessingShader.SetInt("Width", texture1.width);
        PostProcessingShader.SetInt("Height", texture1.height);
    }
    private static void SetShaderVariables1Texture(RenderTexture texture, int kernel)
    {
        PostProcessingShader.SetTexture(kernel, "OutputTexture", texture);

        PostProcessingShader.SetInt("Width", texture.width);
        PostProcessingShader.SetInt("Height", texture.height);
    }
}
