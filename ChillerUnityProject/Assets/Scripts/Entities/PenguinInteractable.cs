using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

//WWorks on a late interaction such that it waits for the next frame to interact
//Only interacts if no other element interacted the previous frame
// Does not have an outline renderer nor hints
public class PenguinInteractable : RoomObjectClass
{
    [Header("Penguin Interactable")]
    public Settings.Controls interactControl;
    public Collider2D interactCollider;
    //If set the mouse must be over the collider
    //If not set, the player must be within the interact distance
    public bool isClickControl = false;
    public Penguin penguin;
    public string[] lowPrompts;
    public string[] medPrompts;
    public string[] highPrompts;
    public AudioClip[] sounds;

    private bool interacting = false;
    private int lastFrame = 0;

    protected override void UpdateRoomObject()
    {
        base.UpdateRoomObject();
        if (interacting)
        {
            if (SettingsInstance.Instance.controlsLastUsedFrame[interactControl] != lastFrame)
            {
                //Nothing else used the control last frame, so use the control
                Interact();
            }
        }
        lastFrame = Time.frameCount;
        interacting = interactControl.GetKeyDown(ignoreIfUsed: true) && !penguin.Locked;
        if (isClickControl)
        {
            interacting &= Util.IsMouseOverObject(interactCollider);
        }
        else
        {
            interacting &= interactCollider.Distance(Player.Instance.getCollider()).distance
                        < Settings.FloatValues.PlayerInteractDistance.Get();
        }
    }

    protected void Interact()
    {
        if(sounds.Length > 0)
        {
            AudioHandler.Instance.playSoundEffect(Util.ChooseRandom(sounds));
        }
        if (Insanity.Instance.IsHigh())
        {
            DialogDisplay.NewDialog(Util.ChooseRandom(highPrompts), AnimationSpriteClass.NULL_STRUCT);
        }
        else if (Insanity.Instance.IsMedium()) 
        {
            DialogDisplay.NewDialog(Util.ChooseRandom(medPrompts), AnimationSpriteClass.NULL_STRUCT);
        }
        else
        {
            DialogDisplay.NewDialog(Util.ChooseRandom(lowPrompts), AnimationSpriteClass.NULL_STRUCT);
        }
    }
}
