using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * the animator for ghost vision UI obj
 * REMEMBER TO INSERT ONE MORE ITEM INTO EACH SPRITE ARRAY
 */
public class GhostVisionUIObjectAnimator : AnimationSpriteClass
{
    [Header("Generics")]
    public GhostVisionUIObject owner;


    // private flags
    protected bool _newActionRequested = false;

    /*
     * after the animation is done, go to next action
     */
    public void UpdateGhostAnimation() {
        base.UpdateAnimation();
        if (currentFrame + 1 >= animationStruct.Length) {
            // prevent ghost disappearing with no despawn animation
            if (! _newActionRequested) {
                _newActionRequested = true;
                owner.changeState();
            }
        }
        else {
            _newActionRequested = false;
        }
    }
}
