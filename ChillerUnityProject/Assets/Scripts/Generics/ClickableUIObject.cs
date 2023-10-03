using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A subclass for ui objects which can be interacted with by clicking
 */
public abstract class ClickableUIObject : UIObjectClass
{
    // The collider for the object where the mouse can click on
    public Collider2D clickableCollider;

    protected override void UpdateUIObject()
    {
        if(Util.GetKeyDownWithMouseOverObject(Settings.Controls.LeftClick, clickableCollider) 
            && Condition())
        {
            Clicked();
        }
    }

    /*
     * The condition for the object to be interactable.
     * Defaults to always if this method is not overriden.
     */
    protected virtual bool Condition() { return true; }
    /*
     * Called when the object is interacted with
     */
    protected abstract void Clicked();
}
