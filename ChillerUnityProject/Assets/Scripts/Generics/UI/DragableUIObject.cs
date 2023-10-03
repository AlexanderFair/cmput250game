using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragableUIObject : ClickReleaseUIObject
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
