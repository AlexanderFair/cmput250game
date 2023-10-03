using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Settings for the game
 */
public static class Settings
{

    public enum Controls
    {
        Interact, LeftClick, Escape
    }

    public enum FloatValues
    {
        PlayerInteractDistance
    }

    private static Dictionary<Controls, KeyCode> controlsSettings = new Dictionary<Controls, KeyCode>
    {
        { Controls.Interact, KeyCode.F },
        { Controls.LeftClick, KeyCode.Mouse0 },
        { Controls.Escape, KeyCode.Escape }
    };

    private static Dictionary<FloatValues, float> floatValueSettings = new Dictionary<FloatValues, float>
    {
        { FloatValues.PlayerInteractDistance, 1.0f }
    };

    /*
     * Returns the KeyCode associated with the Control
     */
    public static KeyCode Get (this Controls control)
    {
        return controlsSettings[control];
    }
    /*
     * Returns the float associated with the FloatValue
     */
    public static float Get (this FloatValues value)
    {
        return floatValueSettings[value]; 
    }
}
