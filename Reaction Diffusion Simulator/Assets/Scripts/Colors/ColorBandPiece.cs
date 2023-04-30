using System;

[Serializable]
public struct ColorBandPiece : IComparable<ColorBandPiece>
{
    public float PositionInLine, R, G, B, A, Bias;
    public int CompareTo(ColorBandPiece other)
    {
        if (PositionInLine > other.PositionInLine)
            return 1;
        else if (other.PositionInLine > PositionInLine)
            return -1;

        return 0;
    }

    public static int Size { get => sizeof(float) * 6; }
}
