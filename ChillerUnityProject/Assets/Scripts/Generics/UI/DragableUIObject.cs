using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A class for objects which can be dragged without physics
 */
public class DragableUIObject : ClickReleaseUIObject, IDragableSprite
{

    protected override void UpdateUIObject()
    {
        base.UpdateUIObject();

        if (isClicked)
        {
            transform.position = Util.GetMouseWorldPoint();
        }
    }

}
