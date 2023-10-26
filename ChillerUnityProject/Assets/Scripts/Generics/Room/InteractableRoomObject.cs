using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    public Settings.Controls interactionControl = Settings.Controls.InteractWithRoom;

    //The sprite renderer which should obtain an outline when the player is near enough
    public SpriteRenderer interactableRenderer;
    public AudioClip interactSound = null;

    [Header("Interactable Dialog Settings")]
    public bool displayDialogOnInteract = false;
    public Sprite[] dialogProfileAnimation = AnimationSpriteClass.NULL_STRUCT;
    public string dialog = "";
    
    [Header("Interactable Hint Settings")]
    // This is the specific interaction hint pop-up object
    public bool shouldDisplayInteractableHint = true;
    // this is in unit of UNITY BLOCKS!
    public static float interactableHintHeightOffset = 0.5f;
    protected Canvas interactableHintCanvas;

    protected override void UpdateRoomObject()
    {
        this.UpdateOutlinableSprite(interactableRenderer);

        // save it to reduce redundant calls
        bool interCondMet = InteractableCondition();
        // update the interactable hint icon ("F")
        handleInteractableHintSprite(interCondMet);
        // handle interaction
        if (interCondMet && interactionControl.GetKeyDown())
        {
            Interact();
        }
    }

    /*
     * The condition for the object to be interactable.
     * Defaults to always if this method is not overriden.
     */
    public virtual bool InteractableCondition() {
        // validate the distance
        bool withinRadius = interactableCollider.Distance(Player.Instance.getCollider()).distance
                        < Settings.FloatValues.PlayerInteractDistance.Get();
    
        return withinRadius; 
    }
    /*
     * This is of internal use
     * The bool parameter should determine if the "F" above it should appear or disappear
     */
    private void handleInteractableHintSprite(bool visible) {
        if (shouldDisplayInteractableHint) {
            // instantiate a new "F" when absent
            if (interactableHintCanvas == null) {
                GameObject createdGameObj = Instantiate(
                    Settings.PrefabObjects.InteractableHint.Get());
                // interactableHint = Instantiate(textbox, textBoxPosition, Quaternion.identity);
                interactableHintCanvas = createdGameObj.GetComponent<Canvas>();
                // set the parent of the hint ("F") to the current attatched object
                interactableHintCanvas.transform.SetParent(transform, false);
                
                // put the "F" in place
                //grabbing desired position
                Vector3 textBoxPosition = transform.position 
                        + (Vector3.up * (interactableHintHeightOffset + interactableCollider.bounds.extents.y) );
                // set its pos to the appropriate place
                Transform Panel = interactableHintCanvas.transform.GetChild(0);
                // if this contains rect transf.
                if (Panel.TryGetComponent<RectTransform>(out RectTransform rctTsf)) {
                    rctTsf.offsetMax = textBoxPosition * 40;
                    rctTsf.offsetMin = rctTsf.offsetMax;
                }
                // otherwise, use position
                else
                    Panel.transform.position = textBoxPosition;
            }

            //display the textbox by using enabled
            interactableHintCanvas.enabled = visible;  
        }
    }

    /*
     * Called when the object is interacted with
     * If overridden, STILL CALL THIS FUNCTION. This will
     * only functionality that is mandatory for interactions.
     */
    protected virtual void Interact(){
        AudioHandler.Instance.playSoundEffect(interactSound);
        if(displayDialogOnInteract)
        {
            DialogDisplay.NewDialog(dialog, dialogProfileAnimation);
        }
    }
}
