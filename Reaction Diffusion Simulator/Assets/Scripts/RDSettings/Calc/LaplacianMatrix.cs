using System;
using UnityEngine;
using TMPro;

public class LaplacianMatrix : MonoBehaviour
{
    [SerializeField] private TMP_InputField bottomLeft, bottom, bottomRight, left, center, right, topLeft, top, topRight;
    public SerializedMatrix3x3 Matrix
    {
        get
        {
            SerializedMatrix3x3 matrix = new SerializedMatrix3x3();

            matrix.Index00 = GameManager.StringToFloat(bottomLeft.text, -1, 1);
            matrix.Index10 = GameManager.StringToFloat(bottom.text, -1, 1);
            matrix.Index20 = GameManager.StringToFloat(bottomRight.text, -1, 1);

            matrix.Index01 = GameManager.StringToFloat(left.text, -1, 1);
            matrix.Index11 = GameManager.StringToFloat(center.text, -1, 1);
            matrix.Index21 = GameManager.StringToFloat(right.text, -1, 1);

            matrix.Index02 = GameManager.StringToFloat(topLeft.text, -1, 1);
            matrix.Index12 = GameManager.StringToFloat(top.text, -1, 1);
            matrix.Index22 = GameManager.StringToFloat(topRight.text, -1, 1);

            return matrix;
        }
    }

    public void SetMatrixSilent(SerializedMatrix3x3 matrix3x3)
    {
        bottomLeft.SetTextWithoutNotify(matrix3x3.Index00.ToString());
        bottom.SetTextWithoutNotify(matrix3x3.Index10.ToString());
        bottomRight.SetTextWithoutNotify(matrix3x3.Index20.ToString());

        left.SetTextWithoutNotify(matrix3x3.Index01.ToString());
        center.SetTextWithoutNotify(matrix3x3.Index11.ToString());
        right.SetTextWithoutNotify(matrix3x3.Index21.ToString());

        topLeft.SetTextWithoutNotify(matrix3x3.Index02.ToString());
        top.SetTextWithoutNotify(matrix3x3.Index12.ToString());
        topRight.SetTextWithoutNotify(matrix3x3.Index22.ToString());
    }
    public void SetMatrixNonSilent(SerializedMatrix3x3 matrix3x3)
    {
        bottomLeft.text = matrix3x3.Index00.ToString();
        bottom.text = matrix3x3.Index10.ToString();
        bottomRight.text = matrix3x3.Index20.ToString();

        left.text = matrix3x3.Index01.ToString();
        center.text = matrix3x3.Index11.ToString();
        right.text = matrix3x3.Index21.ToString();

        topLeft.text = matrix3x3.Index02.ToString();
        top.text = matrix3x3.Index12.ToString();
        topRight.text = matrix3x3.Index22.ToString();
    }
}
