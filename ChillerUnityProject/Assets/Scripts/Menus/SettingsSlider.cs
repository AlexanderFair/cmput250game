using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.UI;

public class SettingsSlider : MenuClickCaller
{
    [Header("Slider Settings")]
    public float minX;
    public float maxX;
    public float minInput;
    public float maxInput;
    public Settings.Controls clickControl;
    
    public MenuClickableObject slider;
    public Settings.FloatValues floatValue;

    public Text valueText;
    public Text keyText;

    public string valueFormat = "0.00";

    private bool clicked = false;

    protected override void AwakeMenuObject()
    {
        base.AwakeMenuObject();
        if(slider == null)
        {
            Settings.DisplayError("Slider is null", gameObject);
            return;
        }

        keyText.text = Regex.Replace(floatValue.ToString(), "(\\B[A-Z])", " $1");
        float val = floatValue.Get();
        valueText.text = val.ToString(valueFormat);
        
        float per = (val - minInput) / (maxInput - minInput);
        float xval = per * (maxX - minX) + minX;
        slider.transform.localPosition = new Vector3(xval, slider.transform.localPosition.y);
    }

    protected override void UpdateMenuObject()
    {
        if (!clicked)
        {
            return;
        }

        if (slider.clickKey.GetKeyUp())
        {
            clicked = false;
        }

        if (clicked)
        {
            UpdateSliderPositionFromMouse();
        }
    }

    public override void OnMenuClick(MenuClickableObject obj)
    {
        clicked = true;
    }

    private void UpdateSliderPositionFromMouse()
    {
        float pos0 = slider.transform.position.x - slider.transform.localPosition.x; // * size?
        float xval = Util.GetMouseWorldPoint().x - pos0;
        xval = Mathf.Clamp(xval, minX, maxX);
        slider.transform.localPosition = new Vector3(xval, slider.transform.localPosition.y);
        float per = (xval - minX) / (maxX - minX);
        float input = per * (maxInput - minInput) + minInput;
        string formattedValue = input.ToString(valueFormat);
        input = float.Parse(formattedValue);
        SettingsInstance.Instance.floatPairings[floatValue] = new SettingsInstance.FloatValue { key = floatValue, value = input };
        valueText.text = formattedValue;
    }

}
