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
    // the boolean representing if the object was placed that frame, otherwise the object is not updated by the manager.
    public bool ChangedThisFrame { get; set; } = false;

    private bool enabledMovement = true;

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

    protected override void MouseUp()
    {
        base.MouseUp();
        ChangedThisFrame = true;
    }

    public override bool ClickableCondition()
    {
        return base.ClickableCondition() && enabledMovement;
    }

    public void EnableMovement(bool enable = true)
    {
        enabledMovement = enable;
    }


}
