using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidableDragableUIObject : ClickReleaseUIObject
{
    public Rigidbody2D rigidBody;

    protected override void UpdateUIObject()
    {
        base.UpdateUIObject();

        if (isClicked)
        {
            rigidBody.MovePosition(Util.GetMouseWorldPoint());
        }
        else
        {
            rigidBody.velocity = Vector2.zero;
        }
    }

}
