using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A class for RoomObjects which display a UI when interacted with
 */
public class DisplayUIRoomObject : InteractableRoomObject
{
    [Header("Display UI Room Settings")]
    // The prefab of the UI object to display
    public GameObject uiPrefab = null;

    // The currently displayed ui
    protected GameObject ui = null;

    public override void Start()
    {
        base.Start();
        if (ui == null)
        {
            Settings.DisplayWarning("UI is null", gameObject);
        }
    }

    /* Called when the object is interacted with */
    protected override void Interact()
    {
        base.Interact();

        if (uiPrefab != null)
        {
            ui = UIObjectClass.InstantiateNewUIElement(uiPrefab);
            DisplayedUI();
        }
    }
    
    /*
     * Called when the object is interacted with and a ui is displayed
     */
    protected virtual void DisplayedUI() { }
}
