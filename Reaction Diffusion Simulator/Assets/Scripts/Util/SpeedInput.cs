using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedInput : MonoBehaviour
{
    private static TMP_InputField field;

    private static int val;
    public static int Value
    {
        get => val;

        set 
        {
            val = Mathf.Max(0, value);
            field.SetTextWithoutNotify(value.ToString());
        } 
    }

    private void Awake()
    {
        field = GetComponent<TMP_InputField>();
        val = (int)Mathf.Max(0, GameManager.StringToFloat(field.text));
    }

    public void OnTextChange() => val = (int)Mathf.Max(0, GameManager.StringToFloat(field.text));
}
