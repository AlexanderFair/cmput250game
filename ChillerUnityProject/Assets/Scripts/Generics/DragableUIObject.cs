using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragableUIObject : ClickableUIObject
{
    protected bool clicked = false;


    protected override void UpdateUIObject()
    {
        base.UpdateUIObject();

        if (Input.GetKeyUp(Settings.Controls.LeftClick))
        {
            clicked = false;
        }

        if (clicked)
        {
            transform.position = Util.GetMouseWorldPoint();
        }
    }

    protected override void Clicked()
    {
        clicked = true;
    }
    
}
