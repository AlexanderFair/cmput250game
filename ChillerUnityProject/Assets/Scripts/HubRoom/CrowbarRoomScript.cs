using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowbarRoomScript : DisplayUIRoomObject
{
    [Header("Crowbar room object")]
    public GameObject completionUI;

    public static bool Complete { get; private set; } = false;
    public static bool HasCrowbar { get; set; } = false;

    public override void Start()
    {
        base.Start();
        if (Complete)
        {
            uiPrefab = completionUI;
        }
    }

    //Called when the room is solved
    public void Solved()
    {
        Complete = true;
        UIObjectClass.InstantiateNewUIElement(completionUI);
        uiPrefab = completionUI;
    }

    protected override void DisplayedUI()
    {
        base.DisplayedUI();
        ui.GetComponent<CrowbarUIScript>()?.Setup(this);
    }

}
