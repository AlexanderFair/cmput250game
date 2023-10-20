using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A class for the key
// If the key snaps to the lock, it is inside the lock.
// When we click the key to drag it, it comes out of the lock
//
// If the cage is unlocked, the key stays trapped inside the lock
public class CageKeyScript : SnapDragUIObject
{
    [Header("Key Settings")]
    //The sprite that should be used when the key is outside of the lock
    public Sprite insideSprite;
    //The sprite that should be used when the key is inside the lock
    public Sprite outsideSprite;
    //The sprite renderer for the key -- should be the same as spriteClickableOutlineRenderer
    public SpriteRenderer spriteRenderer;
    //The ui puzzle object
    public CagePuzzleScript puzzle;


    public override void Snap(Collider2D snap)
    {
        base.Snap(snap);
        spriteRenderer.sprite = insideSprite;
        puzzle.KeyInsideLock(true);
    }

    protected override void MouseDown()
    {
        base.MouseDown();
        spriteRenderer.sprite = outsideSprite;
        puzzle.KeyInsideLock(false);
    }

    public override bool ClickableCondition()
    {
        return base.ClickableCondition() && !CageRoomScript.unlocked;
    }

}
