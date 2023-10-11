using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioUIDialObject : DialUIObject
{
    private float currentValue = 0;

    [Header("Radio UI Dial Settings")]
    public Settings.PrefabAnimations anim;

    private string display = "A paragraph is defined as <b><i>“a group of sentences <color=red>or a single</color> sentence that forms as unit”</i></b> (Lunsford and Connors 116). Length and appearance do not determine whether a section in a paper is a paragraph.";

    protected override float CalcDialOutputValue(float _rot)
    {
        float prevValue = currentValue;
        currentValue = base.CalcDialOutputValue(_rot);

        if(prevValue < 9 && currentValue >= 9)
        {
            DialogDisplay.NewDialog(display, anim.Get());
        }

        if(prevValue >= 9 && currentValue < 9)
        {
            DialogDisplay.StopCurrentDisplay();
        }

        return currentValue;
    }
}
