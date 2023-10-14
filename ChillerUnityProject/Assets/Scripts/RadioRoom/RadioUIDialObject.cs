using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioUIDialObject : DialUIObject
{
    private int currentValue = 0;
    public int Value { get { return currentValue; } }

    [Header("Radio UI Dial Settings")]
    public Settings.PrefabAnimations anim;
    protected override float CalcDialOutputValue(float _rot)
    {
        currentValue = (int)(base.CalcDialOutputValue(_rot) * 100);
        return currentValue/100f;
    }
}
