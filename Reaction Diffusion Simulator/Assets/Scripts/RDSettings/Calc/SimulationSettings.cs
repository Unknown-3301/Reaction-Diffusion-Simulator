using System;

[Serializable]
public struct SimulationSettings
{
    public SerializedMatrix3x3 LaplacianMatrix;
    public float DiffusionA, DiffusionB, KillRate, FeedRate;

    public static int Size { get => sizeof(float) * 13; }
}
