using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FSP_DrawingRadius : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_InputField otherInputField;
    [SerializeField] private ReactionDiffusion rd;

    void Update()
    {
        rd.DrawRadius += (int)Input.mouseScrollDelta.y;
        if (Input.mouseScrollDelta.y != 0)
        {
            inputField.SetTextWithoutNotify(rd.DrawRadius.ToString());
            otherInputField.SetTextWithoutNotify(rd.DrawRadius.ToString());
        }
    }

    private void OnEnable()
    {
        inputField.text = rd.DrawRadius.ToString();
    }

    public void OnEndEdit()
    {
        inputField.SetTextWithoutNotify(Mathf.Max(0, GameManager.StringToFloat(inputField.text)).ToString());
        otherInputField.SetTextWithoutNotify(inputField.text);
    }
}
