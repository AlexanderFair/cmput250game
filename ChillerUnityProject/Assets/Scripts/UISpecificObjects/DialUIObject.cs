using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * A class for a dial which displays a value onto a text element depending on the angle of the dial.
 */
public class DialUIObject : DragToRotateUIObject
{
    [Header("Dial Settings")]
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
    // what sound effect should be played when the rotate this
    public AudioClip spinSoundEffect;
    private float rot=0;
    private float lastDisplayed = 0;
    // if uncapped, trying to play that many sound effects crashes the audio system for a bit lol
    private float howLongBetweenEachSoundEffect = 0.05f;
    private float timeSinceLastSoundEffect = 0f;
    public float distanceBetweenClicks = 0.1f;

    protected override void StartUIObject()
    {
        base.StartUIObject();
        rot = startRelativeRot;
        transform.rotation = Quaternion.Euler(0, 0, -startPhysicalRot);
    }

    protected override void UpdateUIObject()
    {
        timeSinceLastSoundEffect += Time.deltaTime;
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

        float v = CalcDialOutputValue(rot);
        if ((int)(lastDisplayed * 10) % 10 != (int)(v * 10) % 10){
            if (timeSinceLastSoundEffect >= howLongBetweenEachSoundEffect){
                AudioHandler.Instance.playSoundEffect(spinSoundEffect);
                timeSinceLastSoundEffect = 0f;
            }
        }
        lastDisplayed = v;
        DisplayDialOutputValue(v);

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
