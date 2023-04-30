using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FSP_SpecieSelect : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private ReactionDiffusion rd;

    void Update()
    {
        if (SpecieManager.manager == null)
            return;

        if (!GameManager.SelectedInputField)
        {
            int dir = (Input.GetKeyDown(KeyCode.UpArrow) ? 1 : 0) - (Input.GetKeyDown(KeyCode.DownArrow) ? 1 : 0);

            if (dir != 0)
            {
                SpecieManager.SelectSpecie(rd.DrawSpecieIndex + dir);
                inputField.SetTextWithoutNotify((rd.DrawSpecieIndex + 1).ToString());
            }

            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                SpecieManager.SelectSpecie(0);
                inputField.SetTextWithoutNotify((rd.DrawSpecieIndex + 1).ToString());
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                SpecieManager.SelectSpecie(1);
                inputField.SetTextWithoutNotify((rd.DrawSpecieIndex + 1).ToString());
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                SpecieManager.SelectSpecie(2);
                inputField.SetTextWithoutNotify((rd.DrawSpecieIndex + 1).ToString());
            }
            if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
            {
                SpecieManager.SelectSpecie(3);
                inputField.SetTextWithoutNotify((rd.DrawSpecieIndex + 1).ToString());
            }
            if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
            {
                SpecieManager.SelectSpecie(4);
                inputField.SetTextWithoutNotify((rd.DrawSpecieIndex + 1).ToString());
            }
            if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
            {
                SpecieManager.SelectSpecie(5);
                inputField.SetTextWithoutNotify((rd.DrawSpecieIndex + 1).ToString());
            }
            if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7))
            {
                SpecieManager.SelectSpecie(6);
                inputField.SetTextWithoutNotify((rd.DrawSpecieIndex + 1).ToString());
            }
            if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8))
            {
                SpecieManager.SelectSpecie(7);
                inputField.SetTextWithoutNotify((rd.DrawSpecieIndex + 1).ToString());
            }
            if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9))
            {
                SpecieManager.SelectSpecie(8);
                inputField.SetTextWithoutNotify((rd.DrawSpecieIndex + 1).ToString());
            }

        }
    }

    private void OnEnable()
    {
        inputField.text = (rd.DrawSpecieIndex + 1).ToString();
    }

    public void OnValueChange(string t)
    {
        if (SpecieManager.manager != null)
        {
            SpecieManager.SelectSpecie(Mathf.Clamp((int)GameManager.StringToFloat(t) - 1, 0, SpecieManager.NumberOfSpecies - 1));
        }
    }
    public void OnEndEdit()
    {
        if (SpecieManager.manager == null)
        {
            inputField.text = "1";
            return;
        }

        inputField.SetTextWithoutNotify(Mathf.Clamp(GameManager.StringToFloat(inputField.text), 1, SpecieManager.NumberOfSpecies).ToString());
    }
}
