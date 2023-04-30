using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController
{
    Inputs inputActions;

    public ActionButtonState[] Specie1To9Button { get; private set; }
    public ActionButtonState NextSpecieButton { get; set; }
    public ActionButtonState PreviousSpecieButton { get; set; }
    public ActionButtonState PauseButton { get; set; }
    public ActionButtonState EscapeButton { get; set; }
    public ActionButtonState FullscreenButton { get; set; }
    public ActionButtonState SpeedUpButton { get; set; }
    public ActionButtonState SpeedDownButton { get; set; }


    public InputController()
    {
        inputActions = new Inputs();
        Enable();

        Specie1To9Button = new ActionButtonState[9];

        Specie1To9Button[0] = new ActionButtonState(inputActions.Controls.Specie1);
        Specie1To9Button[0].OnPress = () => SpecieManager.SelectSpecie(0);

        Specie1To9Button[1] = new ActionButtonState(inputActions.Controls.Specie2);
        Specie1To9Button[1].OnPress = () => SpecieManager.SelectSpecie(1);

        Specie1To9Button[2] = new ActionButtonState(inputActions.Controls.Specie3);
        Specie1To9Button[2].OnPress = () => SpecieManager.SelectSpecie(2);

        Specie1To9Button[3] = new ActionButtonState(inputActions.Controls.Specie4);
        Specie1To9Button[3].OnPress = () => SpecieManager.SelectSpecie(3);

        Specie1To9Button[4] = new ActionButtonState(inputActions.Controls.Specie5);
        Specie1To9Button[4].OnPress = () => SpecieManager.SelectSpecie(4);

        Specie1To9Button[5] = new ActionButtonState(inputActions.Controls.Specie6);
        Specie1To9Button[5].OnPress = () => SpecieManager.SelectSpecie(5);

        Specie1To9Button[6] = new ActionButtonState(inputActions.Controls.Specie7);
        Specie1To9Button[6].OnPress = () => SpecieManager.SelectSpecie(6);

        Specie1To9Button[7] = new ActionButtonState(inputActions.Controls.Specie8);
        Specie1To9Button[7].OnPress = () => SpecieManager.SelectSpecie(7);

        Specie1To9Button[8] = new ActionButtonState(inputActions.Controls.Specie9);
        Specie1To9Button[8].OnPress = () => SpecieManager.SelectSpecie(8);



        SpeedUpButton = new ActionButtonState(inputActions.Controls.SpeedUp);
        SpeedUpButton.OnPress = () => SpeedInput.Value++;

        SpeedDownButton = new ActionButtonState(inputActions.Controls.SpeedDown);
        SpeedDownButton.OnPress = () => SpeedInput.Value--;
    }

    public void Update()
    {
        if (SpecieManager.manager != null)
        {
            for (int i = 0; i < Specie1To9Button.Length; i++)
            {
                Specie1To9Button[i].Update();

                NextSpecieButton.Update();
                PreviousSpecieButton.Update();
            }

            PauseButton.Update();
            EscapeButton.Update();
            FullscreenButton.Update();
            SpeedUpButton.Update();
            SpeedDownButton.Update();
        }
    }

    public void Enable() => inputActions.Enable();
    public void Disable() => inputActions.Disable();
}
