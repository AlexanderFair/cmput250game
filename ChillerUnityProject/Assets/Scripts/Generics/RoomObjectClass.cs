using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * RoomObjectClass : MonoBehaviour
 * 
 * Represents the base class to be used for any object visible in the room level.
 * Object in the room should extend this class instead of MonoBehaviour
 * 
 * This class controls when the update method is called. If the UI or Menus are active, 
 * any object extending this class will no longer get updated.
 * 
 * Subclasses must implement the UpdateRoomObject. 
 *
 */
public abstract class RoomObjectClass : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(!UIObjectClass.IsUIActive() && !MenuObjectClass.IsMenuActive())
        {
            UpdateRoomObject();
        }
    }

    /* Called when the object is updated and the UI and Menu is not active */
    protected abstract void UpdateRoomObject();
}
