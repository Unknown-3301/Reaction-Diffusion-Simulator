using System;
using UnityEngine;

[Serializable]
public struct SerializedMatrix3x3
{
    public float Index00, Index10, Index20;
    public float Index01, Index11, Index21;
    public float Index02, Index12, Index22;

    public Matrix4x4 ConvertToMatrix() => new Matrix4x4(new Vector4(Index00, Index01, Index02), new Vector4(Index10, Index11, Index12), new Vector4(Index20, Index21, Index22), new Vector4());
}
