using System;
using UnityEngine.InputSystem;

public class ActionButtonState
{
    private InputAction inputAction;
    public ButtonState State { get; private set; }
    public Action OnPress { get; set; }
    public Action OnHold { get; set; }
    public Action OnRelease { get; set; }

    private bool ableToPress;
    private bool alreadyPressed;

    public ActionButtonState(InputAction inputAction)
    {
        this.inputAction = inputAction;
        this.inputAction.started += context => alreadyPressed = true;
        this.inputAction.canceled += context => alreadyPressed = false;

        ableToPress = true;
    }
    public void Update()
    {
        if (ableToPress && !alreadyPressed)
        {
            State = ButtonState.None;
        }
        else if (ableToPress && alreadyPressed)
        {
            ableToPress = false;
            State = ButtonState.Pressed;

            OnPress?.Invoke();
        }
        else if (!ableToPress && alreadyPressed)
        {
            State = ButtonState.Hold;

            OnHold?.Invoke();
        }
        else
        {
            State = ButtonState.Released;
            ableToPress = true;

            OnRelease?.Invoke();
        }
    }
}
