using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * A class for a dial which displays a value onto a text element depending on the angle of the dial.
 */
public class DialUIObject : DragToRotateUIObject
{
    // Where the value is displayed
    public UITextObject textDisplay;
    // the min and max values (inclusve)
    public float max, min;
    // the physical world angle at which startPhysicalRot will be (rotates the whole dial)
    public float startPhysicalRot;
    // the starting angle the the dial will point to (rotates just the numbers but not the arrow)
    public float startRelativeRot = 0;
    // the half range that the dial can turn. A value of x means that the dial can go from -x to x (inclusive)
    public float rotationRange;
    // the display format of the value
    public string valueFormat = "0.00";
    //How much the rotation difference is scaled by, should be 1 (for counterclock) or -1 (for clock)
    public float rotDifferenceScalar = 1;

    private float rot=0;

    public void Start()
    {
        rot = startRelativeRot;
        transform.rotation = Quaternion.Euler(0, 0, -startPhysicalRot);
    }

    protected override void UpdateUIObject()
    {

        float prevRot = transform.rotation.eulerAngles.z;
        base.UpdateUIObject();
        float afterRot = transform.rotation.eulerAngles.z;

        float angleDifference = afterRot - prevRot;
        if(angleDifference > 180) { angleDifference = -360 + angleDifference; } //Assumes that you cant move more that 180 in a frame, this can create some fun glitches
        if(angleDifference < -180) { angleDifference = 360 + angleDifference; }

        rot += angleDifference * rotDifferenceScalar;

        if (rot < -rotationRange)
        {
            rot = -rotationRange;
            transform.rotation = Quaternion.Euler(0, 0, -rotationRange - startPhysicalRot - startRelativeRot);
        }
        if(rot > rotationRange)
        {
            rot = rotationRange;
            transform.rotation = Quaternion.Euler(0, 0, rotationRange - startPhysicalRot - startRelativeRot);
        }
        DisplayDialOutputValue(CalcDialOutputValue(rot));

    }
    // Returns the value that should be associated with the relative rotation
    protected virtual float CalcDialOutputValue(float _rot)
    {
        return (rot + rotationRange) / 2f * (max - min) / rotationRange + min;
    }

    // Called to update the text display
    protected virtual void DisplayDialOutputValue(float v)
    {
        textDisplay.SetText(v.ToString(valueFormat));
    }

}
