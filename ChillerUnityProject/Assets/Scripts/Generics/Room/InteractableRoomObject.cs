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
public abstract class InteractableRoomObject : RoomObjectClass, IInteractableSprite 
{

    [Header("Interactable Room Settings")]
    /* The collider for the obect */
    public Collider2D interactableCollider;

    public Settings.Controls interactionControl = Settings.Controls.Interact;

    //The sprite renderer which should obtain an outline when the player is near enough
    public SpriteRenderer interactableRenderer;
    public AudioClip interactSound = null;

    protected override void UpdateRoomObject()
    {
        this.UpdateOutlinableSprite(interactableRenderer);

        if (Input.GetKeyDown(interactionControl.Get()) && InteractableCondition())
        {
            Interact();
        }
    }

    /*
     * The condition for the object to be interactable.
     * Defaults to always if this method is not overriden.
     */

    public virtual bool InteractableCondition() {
        return interactableCollider.Distance(Player.Instance.getCollider()).distance
                        < Settings.FloatValues.PlayerInteractDistance.Get(); 
    }
    /*
     * Called when the object is interacted with
     * If overridden, STILL CALL THIS FUNCTION. This will
     * only functionality that is mandatory for interactions.
     */
    protected virtual void Interact(){
        if ((object)interactSound != null) {
            AudioHandler.Instance.playSoundEffect(interactSound);
        }
    }

    

}
