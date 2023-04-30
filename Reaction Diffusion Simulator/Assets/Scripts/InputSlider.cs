using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputSlider : MonoBehaviour
{
    [SerializeField] private int roundDigits = 5;
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_InputField inputField;

    public float Value { get => slider.value; }

    public void SetValueSilent(float newValue) => slider.SetValueWithoutNotify(newValue);
    public void SetValueNonSilent(float newValue) => slider.value = newValue;
    void Update()
    {
        if (inputField.isFocused)
        {
            slider.value = GameManager.StringToFloat(inputField.text);
        }
        else
        {
            inputField.text = Math.Round(slider.value, roundDigits).ToString();
        }
    }
}
