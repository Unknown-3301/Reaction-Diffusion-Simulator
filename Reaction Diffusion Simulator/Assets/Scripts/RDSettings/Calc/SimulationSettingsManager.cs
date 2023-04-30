using System;
using UnityEngine;

public class SimulationSettingsManager : MonoBehaviour
{
    private static SimulationSettingsManager manager;

    [SerializeField] private InputSlider diffusionA, diffusionB, killRate, feedRate;
    [SerializeField] private LaplacianMatrix matrix;

    private bool changedThisFrame;

    public static Action OnChange { get; set; }
    public static SimulationSettings Settings
    {
        get => new SimulationSettings()
        {
            LaplacianMatrix = manager.matrix.Matrix,
            DiffusionA = manager.diffusionA.Value,
            DiffusionB = manager.diffusionB.Value,
            KillRate = manager.killRate.Value,
            FeedRate = manager.feedRate.Value,
        };
    }

    private void Awake() => manager = this;
    private void Update() => changedThisFrame = false;

    public void OnValueChange()
    {
        if (changedThisFrame)
            return;

        if (OnChange != null)
        {
            OnChange();
        }

        changedThisFrame = true;
    }

    public static void SetSettingsSilent(SimulationSettings newSettings)
    {
        manager.matrix.SetMatrixSilent(newSettings.LaplacianMatrix);

        manager.diffusionA.SetValueSilent(newSettings.DiffusionA);
        manager.diffusionB.SetValueSilent(newSettings.DiffusionB);
        manager.killRate.SetValueSilent(newSettings.KillRate);
        manager.feedRate.SetValueSilent(newSettings.FeedRate);
    }
    public static void SetSettingsNonSilent(SimulationSettings newSettings)
    {
        manager.matrix.SetMatrixSilent(newSettings.LaplacianMatrix);

        manager.diffusionA.SetValueSilent(newSettings.DiffusionA);
        manager.diffusionB.SetValueSilent(newSettings.DiffusionB);
        manager.killRate.SetValueSilent(newSettings.KillRate);
        manager.feedRate.SetValueSilent(newSettings.FeedRate);

        manager.OnValueChange();
    }
}
