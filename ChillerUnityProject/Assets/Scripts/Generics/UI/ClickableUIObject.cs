using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A subclass for ui objects which can be interacted with by clicking
 */
public abstract class ClickableUIObject : UIObjectClass, IClickableSprite
{
    [Header("Clickable UI Settings")]
    // The collider for the object where the mouse can click on
    public Collider2D clickableCollider;

    // The sprite renderer which should obtain an outline when the sprite can be clicked
    public SpriteRenderer spriteClickableOutlineRenderer;

    public Settings.Controls clickControl = Settings.Controls.LeftClick;

    protected override void UpdateUIObject()
    {
        this.UpdateOutlinableSprite(spriteClickableOutlineRenderer);

        if(Util.GetKeyDownWithMouseOverObject(clickControl, clickableCollider) 
            && ClickableCondition())
        {
            Clicked();
        }
    }

    /*
     * The condition for the object to be interactable.
     * Defaults to always if this method is not overriden.
     */
    public virtual bool ClickableCondition() { return true; }
    /*
     * Called when the object is interacted with
     */
    protected abstract void Clicked();
}
