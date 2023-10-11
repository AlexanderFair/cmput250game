using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A class which rotates towards the mouse when the mouse is pressed down on it
 */
public class DragToRotateUIObject : ClickReleaseUIObject, IDragableSprite
{
    protected override void UpdateUIObject()
    {
        base.UpdateUIObject();

        if (!isClicked || !RotateCondition())
        {
            return;
        }

        Vector2 targetPosition = Util.GetMouseWorldPoint();
        Vector2 direction = targetPosition - (Vector2)transform.position;
        float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }

    /*
     * A condition for if the object should rotate this frame
     */
    protected virtual bool RotateCondition() { return true; }
}
