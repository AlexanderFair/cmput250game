using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// class on the lock, allows the key to snap to it.
//If when awaoken the object is unlocked, makes the key start snapped
public class CageLockedScript : SnapManagerUIObject
{
    protected override void StartUIObject()
    {
        base.StartUIObject();
        if (CageRoomScript.unlocked)
        {
            dragableTiles[0].Snap(snapColliders[0]);
        }
    }
}
