using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A class for objects which can be dragged with collision physics
 */
public class CollidableDragableUIObject : ClickReleaseUIObject, IDragableSprite
{
    [Header("Collidable Dragable UI Settings")]
    public Rigidbody2D rigidBody;

    protected override void UpdateUIObject()
    {
        base.UpdateUIObject();

        if (isClicked)
        {
            rigidBody.MovePosition(Util.GetMouseWorldPoint());
        }
    }

}
