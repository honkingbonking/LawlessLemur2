using WiimoteApi;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    Wiimote mote;
    GuitarData gd;

    // Variables to store the previous state of each button
    bool prevGreenState = false;
    bool prevRedState = false;
    bool prevYellowState = false;
    bool prevBlueState = false;
    bool prevOrangeState = false;
    bool prevStrumUpState = false;
    bool prevStrumDownState = false;
    bool prevMinusState = false;
    bool prevPlusState = false;

    void Start()
    {
        WiimoteManager.FindWiimotes();
        mote = WiimoteManager.Wiimotes[0];
        if (mote != null) { Debug.Log("Connected!"); }
        mote.SendPlayerLED(true, false, false, false);

        Debug.Log("Current Extension: " + mote.current_ext.ToString());
    }

    void FixedUpdate()
    {
        // Make sure to read Wiimote data first
        mote.ReadWiimoteData();

        if (mote.current_ext == ExtensionController.GUITAR)
        {
            gd = mote.Guitar;

            if (gd != null)
            {
                // Check each button individually
                CheckButtonState("Green", gd.green, ref prevGreenState);
                CheckButtonState("Red", gd.red, ref prevRedState);
                CheckButtonState("Yellow", gd.yellow, ref prevYellowState);
                CheckButtonState("Blue", gd.blue, ref prevBlueState);
                CheckButtonState("Orange", gd.orange, ref prevOrangeState);
                CheckButtonState("Strum Up", gd.strum_up, ref prevStrumUpState);
                CheckButtonState("Strum Down", gd.strum_down, ref prevStrumDownState);
                CheckButtonState("Minus", gd.minus, ref prevMinusState);
                CheckButtonState("Plus", gd.plus, ref prevPlusState);
            }
        }
    }

    void CheckButtonState(string buttonName, bool buttonState, ref bool prevState)
    {
        if (buttonState && !prevState)
        {
            Debug.Log(buttonName + " button pressed");
        }

        // Update the previous state for the next check
        prevState = buttonState;
    }
}
