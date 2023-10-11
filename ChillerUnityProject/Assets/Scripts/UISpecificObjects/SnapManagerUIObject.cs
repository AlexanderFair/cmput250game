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

    private List<int>[] snapedTiles;
    private Dictionary<int, int?> snappedObjects = new Dictionary<int, int?>();

    protected override void AwakeUIObject()
    {
        base.AwakeUIObject();
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
        for(int i=0; i<dragableTiles.Length; i++)
        {
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
                        snapedTiles[j].Add(i);
                        snappedObjects[i] = j;
                        dragableTiles[i].Snap(snapColliders[j]);
                        goto nextElement;
                    }
                }
            }

            nextElement:;
        }
    }
}
