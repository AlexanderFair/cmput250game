using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A class for RoomObjects which display a UI when interacted with
 */
public class DisplayUIRoomObject : InteractableRoomObject
{
    // The prefab of the UI object to display
    public GameObject uiPrefab;
    // The currently displayed ui
    protected GameObject ui = null;

    protected override void Interact()
    {
        ui = UIObjectClass.InstantiateNewUIElement(uiPrefab);
        DisplayedUI();
    }
    /*
     * Called when the object is interacted with and a ui is displayed
     */
    protected virtual void DisplayedUI() { }
}
