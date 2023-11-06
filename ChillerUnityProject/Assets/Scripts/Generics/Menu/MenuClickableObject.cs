using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

/*
 * A menu object which can be clicked
 */
public class MenuClickableObject : MenuObjectClass, IClickableSprite
{
    [Header("Menu Clickable Settings")]
    public Settings.Controls clickKey = Settings.Controls.Click;
    //The area where the user can click
    public Collider2D clickCollider;
    // the menu object which should be waiting for the click
    public MenuClickCaller menuObject;

    // The sprite renderer which should obtain an outline when the sprite is clickable
    public SpriteRenderer clickableRenderer;

    protected override void StartMenuObject()
    {
        base.StartMenuObject();
        this.StartOutlinableSprite(clickableRenderer);
    }
    protected override void UpdateMenuObject()
    {
        this.UpdateOutlinableSprite();

        if (Util.GetKeyDownWithMouseOverObject(clickKey, clickCollider, true) && ClickableCondition())
        {
            clickKey.UseControl(true);
            menuObject.OnMenuClick(this);
        }
    }

    // Returns true if the sprite can be clicked
    public virtual bool ClickableCondition()
    {
        return true;
    }
}

public abstract class MenuClickCaller : MenuObjectClass
{
    // Called when the sprite is clicked
    public abstract void OnMenuClick(MenuClickableObject obj);
}
