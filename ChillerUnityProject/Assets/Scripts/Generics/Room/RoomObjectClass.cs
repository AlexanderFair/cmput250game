using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * RoomObjectClass : MonoBehaviour
 * 
 * Represents the base class to be used for any object visible in the room level.
 * Object in the room should extend this class instead of MonoBehaviour
 * 
 * This class controls when the update method is called. If the UI or Menus are active, 
 * any object extending this class will no longer get updated.
 * 
 * Subclasses must implement the UpdateRoomObject. 
 *
 */
public abstract class RoomObjectClass : MonoBehaviour
{
    [Header("Room Base Object Settings")]
    // Animations
    public List<AnimationSpriteClass> spriteAnimations = new List<AnimationSpriteClass>();

    // Returns true if the RoomObjectClass will update
    public static bool CanUpdate()
    {
        return !UIObjectClass.IsUIActive() && !MenuObjectClass.IsMenuActive();
    }

    public virtual void Start()
    {
        foreach (var anim in spriteAnimations)
        {
            anim?.AwakeAnimation();
        }
    }

    public virtual void OnDestroy()
    {
        foreach(var anim in spriteAnimations)
        {
            anim?.PauseAnimation();
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(CanUpdate())
        {
            UpdateRoomObject();
            foreach (var anim in spriteAnimations)
            {
                anim?.UpdateAnimation();
            }
        }
    }

    /* Called when the object is updated and the UI and Menu is not active */
    protected virtual void UpdateRoomObject() { }
}
