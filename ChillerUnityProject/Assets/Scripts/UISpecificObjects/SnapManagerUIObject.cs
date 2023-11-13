using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

/*
 * A class which manages snappable tiles and the locations they can snap to
 * 
 * If the two colliders overlap, the tiles position becomes that of the location collider
 */
public class SnapManagerUIObject : UIObjectClass
{
    [Header("Snap Manager UI Settings")]
    // The movable tiles
    public SnapDragUIObject[] dragableTiles;
    // The locations where tiles can snap to
    public Collider2D[] snapColliders;
    // The number of tiles that can be snapped to one location at a time
    public int maxTilesPerCollider=1;

    //collider index to list of snap index
    protected List<int>[] snapedTiles;
    //snap index to collider inex
    protected Dictionary<int, int?> snappedObjects = new Dictionary<int, int?>();

    protected override void StartUIObject()
    {
        base.StartUIObject();
        snapedTiles = new List<int>[snapColliders.Length];
        for(int i=0;  i<snapColliders.Length; i++)
        {
            snapedTiles[i] = new List<int>();
        }
        for(int i=0; i<dragableTiles.Length; i++)
        {
            snappedObjects[i] = null;
        }
    }

    protected override void UpdateUIObject()
    {
        bool changed = false;
        for(int i=0; i<dragableTiles.Length; i++)
        {
            if (!dragableTiles[i].ChangedThisFrame)
            {
                continue;
            }
            changed = true;
            dragableTiles[i].ChangedThisFrame = false;
            int? v = snappedObjects[i];
            snappedObjects[i] = null;
            if(v != null)
            {
                snapedTiles[(int)v].Remove(i);
            }

            if (!dragableTiles[i].IsMoving())
            {
                for (int j = 0; j<snapColliders.Length; j++)
                {
                    if (dragableTiles[i].snapCollider.Distance(snapColliders[j]).isOverlapped
                            && snapedTiles[j].Count < maxTilesPerCollider)
                    {
                        ForceSnap(i, j);
                        goto nextElement;
                    }
                }
            }

            nextElement:;
        }

        if (changed)
        {
            ItemsChanged();
        }
    }

    public void ForceSnap(int snap, int collider, bool disable = false)
    {
        snapedTiles[collider].Add(snap);
        snappedObjects[snap] = collider;
        dragableTiles[snap].Snap(snapColliders[collider]);
        if (disable)
        {
            dragableTiles[snap].EnableMovement(false);
        }
    }

    //called when one or more snaps have possibly changed location
    protected virtual void ItemsChanged()
    {

    }
}
