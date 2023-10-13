using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionUIObject : UIObjectClass
{
    [Header("Vision UI Settings")]
    public int mainAnimationIndex;

    private bool changedFrom0Frame = false;

    protected override void AwakeUIObject()
    {
        base.AwakeUIObject();
        if(spriteAnimations.Count == 0 || mainAnimationIndex >= spriteAnimations.Count)
        {
            throw new System.IndexOutOfRangeException("The mainAnimationIndex is greater than the count of sprite anaimations for " + name);
        }
    }

    protected override void UpdateUIObject()
    {
        base.UpdateUIObject();
        if (spriteAnimations[mainAnimationIndex].CurrentFrame == 0 && changedFrom0Frame)
        {
            UIObjectClass.DestroyUIObject(gameObject);
        }
        else if(spriteAnimations[mainAnimationIndex].CurrentFrame != 0)
        {
            changedFrom0Frame = true;
        }
    }
}
