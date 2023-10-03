using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A class for ui objects which have an event when the control occurs ontop of them
 * and an event when the control is released afterwards
 * 
 * Example, draging - the elemtn starts following when clicked, and stops following when released.
 */
public abstract class ClickReleaseUIObject : ClickableUIObject
{
    /*
     * True if the control was pressed overtop of the object and has not been released
     */
    protected bool isClicked = false;


    protected override void UpdateUIObject()
    {
        base.UpdateUIObject();

        if (Input.GetKeyUp(clickControl.Get()))
        {
            MouseUp();
            isClicked = false;
        }
    }

    /*
     * Called when the control is pressed overtop of the collider
     */
    protected virtual void MouseDown() { }

    /*
     * Called when the control is released after previously being pressed ontop of the collider
     */
    protected virtual void MouseUp() { }

    protected sealed override void Clicked()
    {
        isClicked = true;
        MouseDown();
    }
}
