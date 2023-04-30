using System;

[Serializable]
public struct BloomSettings
{
    public bool Enabled;

    public float BloomThreshold;
    public float BloomBrightness;

    public int BlurScale;
    public float BlurSamples;
    public float BlurSD;
}
