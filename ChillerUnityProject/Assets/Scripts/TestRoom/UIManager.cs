using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : ClickableUIObject
{

    private MovingButton _roomObjectClass;
    public MovingButton RoomObjectClass { set { _roomObjectClass = value; } }


    // Update is called once per frame
    protected override void UpdateUIObject()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("clearing");
            ClearUI();
        }

        base.UpdateUIObject();
    }

    protected override void Clicked()
    {
        _roomObjectClass.FinishedUI(gameObject);
    }
}
