using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dial : DragToRotateUIObject
{
    public UITextObject valueDisplay;

    public float min=0, max=360;

    protected override void UpdateUIObject()
    {
        base.UpdateUIObject();
        float afterRot = transform.rotation.eulerAngles.z;
        float value = afterRot * 1f/360 * (max - min) + min;
        valueDisplay.SetText(value.ToString("n2"));

    }
}
