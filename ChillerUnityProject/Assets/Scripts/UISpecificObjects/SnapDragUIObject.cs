using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * A class for tiles which can be dragged an snapped to locations
 */
public class SnapDragUIObject : DragableUIObject
{
    [Header("Snap Drag UI Settings")]
    //The collider that must overlap with the location collider before the object can snap
    public Collider2D snapCollider;

    // Called when the object should snap to a location (this will happen every frame once the object is snapped)
    public virtual void Snap(Collider2D snap)
    {
        transform.position = snap.transform.position;
    }

    // Returns true if the object is being dragged
    public bool IsMoving()
    {
        return isClicked;
    }
}
