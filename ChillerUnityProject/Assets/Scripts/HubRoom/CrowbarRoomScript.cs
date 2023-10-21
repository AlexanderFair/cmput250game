using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowbarRoomScript : DisplayUIRoomObject
{
    public static bool Complete { get; private set; } = false;

    //Called when the room is solved
    public void Solved()
    {
        Complete = true;
        Debug.Log("Solved");
    }

    protected override void DisplayedUI()
    {
        base.DisplayedUI();
        ui.GetComponent<CrowbarUIScript>().Setup(this);
    }

}
