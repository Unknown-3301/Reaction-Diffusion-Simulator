using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FSP_Speed : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private ReactionDiffusion rd;

    void Update()
    {
        if (!GameManager.SelectedInputField)
        {
            int dir = (Input.GetKeyDown(KeyCode.RightArrow) ? 1 : 0) - (Input.GetKeyDown(KeyCode.LeftArrow) ? 1 : 0);

            if (dir != 0)
            {
                SpeedInput.Value = rd.Speed + dir;
                rd.ChangeSpeed(SpeedInput.Value.ToString());
                inputField.SetTextWithoutNotify(SpeedInput.Value.ToString());
            }

            if (Input.GetKeyDown(KeyCode.Space))
                rd.Paused = !rd.Paused;
        }

        
    }

    private void OnEnable()
    {
        inputField.text = SpeedInput.Value.ToString();
    }

    public void OnEndEdit()
    {
        int num = Mathf.Max(0, (int)GameManager.StringToFloat(inputField.text));
        inputField.SetTextWithoutNotify(num.ToString());
        SpeedInput.Value = num;
    }
}
