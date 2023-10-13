using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * An object which allows for a control to be changed in the settings menu
 * When clicked on it waits for the next input to registar and changes the control
 * to that keycode.
 */
public class SettingControlsChanger : MenuClickCaller
{
    public Text key;
    public Text value;

    public Settings.Controls control;

    private bool clicked = false;
    private bool clickedThisFrame = false;
    private bool foundThisFrame = false;

    void Start()
    {
        key.text = control.ToString();
        UpdateText(); 
    }

    public override void OnMenuClick(MenuClickableObject obj)
    {
        if (!clicked && !foundThisFrame)
        {
            clicked = true;
            clickedThisFrame = true;
            UpdateText();
        }
    }

    protected override void UpdateMenuObject()
    {
        foundThisFrame = false;
        if (clicked && !clickedThisFrame)
        {
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    clicked = false;
                    ChangeValue(key);
                    foundThisFrame = true;
                    return;
                }
            }
        }
        clickedThisFrame = false;
    }

    private void ChangeValue(KeyCode newKey)
    {
        var struc = SettingsInstance.Instance.controlsPairings[control];
        struc.value = newKey;
        SettingsInstance.Instance.controlsPairings[control] = struc;
        UpdateText();
    }

    private void UpdateText()
    {
        if (clicked)
        {
            value.text = "";
        }
        else
        {
            value.text = control.GetKeyCode().ToString();
        }
    }
}
