using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BloomManager : MonoBehaviour
{
    private static BloomManager manager;

    [SerializeField] private Toggle enableToggle;

    [SerializeField] private InputSlider threshold;
    [SerializeField] private InputSlider brightness;

    [SerializeField] private InputSlider blurScale;
    [SerializeField] private InputSlider blurSamples;
    [SerializeField] private InputSlider blurSD;

    private void Awake() => manager = this;

    public static bool Enabled { get => manager.enableToggle.isOn; set => manager.enableToggle.isOn = value; }
    public static BloomSettings Settings
    {
        get => new BloomSettings()
        {
            Enabled = Enabled,
            BloomThreshold = manager.threshold.Value,
            BloomBrightness = manager.brightness.Value,
            BlurSamples = manager.blurSamples.Value,
            BlurScale = (int)manager.blurScale.Value,
            BlurSD = manager.blurSD.Value,
        };
    }
    
    public static void SetSettingsSilent(BloomSettings newSettings)
    {
        Enabled = newSettings.Enabled;

        manager.threshold.SetValueSilent(newSettings.BloomThreshold);
        manager.brightness.SetValueSilent(newSettings.BloomBrightness);

        manager.blurScale.SetValueSilent(newSettings.BlurScale);
        manager.blurSamples.SetValueSilent(newSettings.BlurSamples);
        manager.blurSD.SetValueSilent(newSettings.BlurSD);
    }
    public static void SetSettingsNonSilent(BloomSettings newSettings)
    {
        Enabled = newSettings.Enabled;

        manager.threshold.SetValueNonSilent(newSettings.BloomThreshold);
        manager.brightness.SetValueNonSilent(newSettings.BloomBrightness);

        manager.blurScale.SetValueNonSilent(newSettings.BlurScale);
        manager.blurSamples.SetValueNonSilent(newSettings.BlurSamples);
        manager.blurSD.SetValueNonSilent(newSettings.BlurSD);
    }
}
