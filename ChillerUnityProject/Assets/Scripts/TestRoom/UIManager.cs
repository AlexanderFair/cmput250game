using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : UIObjectClass
{

    private MovingButton _roomObjectClass;
    public MovingButton RoomObjectClass { set { _roomObjectClass = value; } }

    private Collider2D btn;

    public override void AwakeUIObject()
    {
        btn = transform.Find("UIButton").GetComponent<Collider2D>();
    }


    // Update is called once per frame
    public override void UpdateUIObject()
    {
        if (Util.GetKeyDownWithMouseOverObject(KeyCode.Mouse0, btn))
        {
            _roomObjectClass.FinishedUI(gameObject);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("clearing");
            ClearUI();
        }
    }
}
