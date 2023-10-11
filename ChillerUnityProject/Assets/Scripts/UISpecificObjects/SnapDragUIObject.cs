using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapDragUIObject : DragableUIObject
{
    [Header("Snap Drag UI Settings")]
    public Collider2D snapCollider;

    public void Snap(Collider2D snap)
    {
        transform.position = snap.transform.position;
    }

    public bool IsMoving()
    {
        return isClicked;
    }
}
