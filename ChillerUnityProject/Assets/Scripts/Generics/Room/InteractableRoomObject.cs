using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A subclass for RoomObjects which can be interacted with
 * 
 * this object can only be interacted with if the Interact key is down,
 * the distance between this collider and the player collider is less
 * than the InteractDistance and the Condition method evaluates to true.
 */
public abstract class InteractableRoomObject : RoomObjectClass
{
    /* The collider for the obect */
    public Collider2D interactableCollider;

    public Settings.Controls interactionControl = Settings.Controls.Interact;

    private bool _interactable = false;
    public bool Interactable { get { return _interactable; } }

    protected override void UpdateRoomObject()
    {
        _interactable = interactableCollider.Distance(Player.plyInstance.getCollider()).distance
                        < Settings.FloatValues.PlayerInteractDistance.Get();
        if (Input.GetKeyDown(interactionControl.Get()) &&
            _interactable && Condition())
        {
            Interact();
        }
    }

    /*
     * The condition for the object to be interactable.
     * Defaults to always if this method is not overriden.
     */
    protected virtual bool Condition() { return true; }
    /*
     * Called when the object is interacted with
     */
    protected abstract void Interact();

}
