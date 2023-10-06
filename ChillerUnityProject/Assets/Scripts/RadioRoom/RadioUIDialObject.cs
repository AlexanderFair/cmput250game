using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioUIDialObject : DialUIObject
{
    private float currentValue = 0;

    protected override float CalcDialOutputValue(float _rot)
    {
        currentValue = base.CalcDialOutputValue(_rot);
        return currentValue;
    }
}
