using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowbarRoomScript : DisableInteractableRoomObject
{
    [Header("Crowbar room object")]
    public GameObject puzzleUI;
    public GameObject completionUI;

    public static bool Complete { get; private set; } = false;
    public static bool HasCrowbar { get; set; } = false;

    private GameObject uiPrefab;

    public override void Start()
    {
        base.Start();
        if (Complete)
        {
            uiPrefab = completionUI;
            DisableInteract();
        }
        else
        {
            uiPrefab = puzzleUI;
        }
    }

    //Called when the room is solved
    public void Solved()
    {
        Complete = true;
        uiPrefab = completionUI;
        DisableInteract();
        UIObjectClass.InstantiateNewUIElement(completionUI);
        
    }

    protected override void Interact()
    {
        base.Interact();
        if (uiPrefab != null)
        {
            GameObject ui = UIObjectClass.InstantiateNewUIElement(uiPrefab);
            ui.GetComponent<CrowbarUIScript>()?.Setup(this);
        }
    }

}
