using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A class which has an event triggered by another collision box overlapping a collision box
 */
public abstract class CollisionInteractableRoomObject : RoomObjectClass, ICollisionInteractionSprite
{
    [Header("Collision Interactable Settings")]
    // The collider that objects can colide with
    public Collider2D thisObjectCollisionBox;
    // The colliders of other objects that can trigger a call
    public List<Collider2D> movingColliders;
    // If the collider should be triggered by the player
    public bool isTriggeredByPlayer = true;

    public override void Start()
    {
        base.Start();
        if(thisObjectCollisionBox == null)
        {
            Settings.DisplayWarning("Collision Box is null", gameObject);
        }

        if(isTriggeredByPlayer)
        {
            movingColliders.Add(Player.Instance.getCollider());
        }

        if(movingColliders.Count == 0)
        {
            Settings.DisplayWarning("No Objects are set to collide with this", gameObject);
        }
    }
    protected override void UpdateRoomObject()
    {
        if (!CollisionInteractionCondition())
        {
            return;
        }
        foreach(Collider2D coll in movingColliders)
        {
            ColliderDistance2D dist = thisObjectCollisionBox.Distance(coll);
            if (!dist.isValid)
            {
                Settings.DisplayWarning("The collider from " + coll.name + " is not a valid collider with " + thisObjectCollisionBox.name, gameObject);
                continue;
            }
            if (dist.isOverlapped)
            {
                Collision(coll);
            }
        }

    }

    //Called when a collider is overlapping with the object
    protected abstract void Collision(Collider2D coll);

    public virtual bool CollisionInteractionCondition() { return true; }
}
