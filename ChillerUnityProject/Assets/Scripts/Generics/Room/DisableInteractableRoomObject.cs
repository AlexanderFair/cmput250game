using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A sub class of InteractableRoomObject such that the interaction can be disabled perminantely (until destory)
 * The object can then have two animations, one when it is not perminantely disabled and one when it is
 */
public class DisableInteractableRoomObject : InteractableRoomObject
{
    [Header("Disable Interact Settings")]
    /*
     * If this has no sprites, no sprite changes when the interact is disabled
     * If there is a sprite but, disabledAnimatorIndex is negative or greater than the length of sprite animators
     *      the interactablerenderer is changed to the first sprite in the list
     * Otherwise the animator is changed
     */
    public Sprite[] disabledAnimation;
    public int disabledAnimatorIndex = -1;
    public bool disableInteractionMethod = true;

    protected bool disabledInteract = false;

    public void DisableInteract()
    {
        disabledInteract = true;
        if(disabledAnimation.Length > 0 )
        {
            if(disabledAnimatorIndex >= 0 && disabledAnimatorIndex < spriteAnimators.Count)
            {
                spriteAnimators[disabledAnimatorIndex].ChangeAnimation(disabledAnimation);
            }
            else
            {
                interactableRenderer.sprite = disabledAnimation[0];
            }
        }
    }

    public override bool InteractableCondition()
    {
        return base.InteractableCondition() && (!disableInteractionMethod || !disabledInteract);
    }
}
