using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionUIObject : RoomObjectClass
{
    [Header("Vision UI Settings")]
    public int mainAnimationIndex;
    public AudioClip[] clipOnCreation;
    public bool pauseAtUI = false;
    public bool destroyOnMenuOpen = true;

    private bool changedFrom0Frame = false;

    public override void Start()
    {
        base.Start();
        AudioHandler.Instance.playSoundEffect(Util.ChooseRandom(clipOnCreation));
        if (spriteAnimators.Count == 0 || mainAnimationIndex >= spriteAnimators.Count)
        {
            DestroyImmediate(gameObject);
            throw new System.IndexOutOfRangeException("The mainAnimationIndex is greater than the count of sprite anaimations for " + name);
        }
    }

    public override void Update()
    {
        if (MenuObjectClass.IsMenuActive())
        {
            if (destroyOnMenuOpen) DestroyImmediate(this);
            return;
        }

        if (CanUpdate() || !pauseAtUI)
        {
            UpdateRoomObject();
            foreach (var anim in spriteAnimators)
            {
                anim?.UpdateAnimation();
            }
        }
    }

    protected override void UpdateRoomObject()
    {
        base.UpdateRoomObject();
        if (spriteAnimators[mainAnimationIndex].CurrentFrame == 0 && changedFrom0Frame)
        {
            UIObjectClass.DestroyUIObject(gameObject);
        }
        else if (spriteAnimators[mainAnimationIndex].CurrentFrame != 0)
        {
            changedFrom0Frame = true;
        }
    }
}
