using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The class that handles the ui cage puzzle
 */
public class CagePuzzleScript : ClickableUIObject
{
    [Header("Cage Puzzle")]
    //The text that should be dsiplayed on the sprite when the cage is locked
    public GameObject lockedText;
    //The text that should be dsiplayed on the sprite when the cage is locked
    public GameObject unlockedText;
    //The clip that should be played when the puzzle is unlocked
    public AudioClip unlockedClip;

    // The room object for the cage
    private CageRoomScript room;
    //The current displayed text object
    private GameObject textobj;
    private bool keyInLock = false;

    public void Setup(CageRoomScript _room)
    {
        room = _room;
        UpdateUnlockStatus();
    }

    protected override void Clicked()
    {
        AudioHandler.Instance.playSoundEffect(unlockedClip);
        room.UnlockCage();
        UpdateUnlockStatus();
    }

    private void UpdateUnlockStatus()
    {
        if(textobj!=null) DestroyUIObject(textobj);
        if (CageRoomScript.unlocked)
        {
            textobj = InstantiateUIElement(unlockedText);
        }
        else
        {
            textobj = InstantiateUIElement(lockedText);
        }
    }

    public void KeyInsideLock(bool isInside)
    {
        keyInLock = isInside;
        Debug.Log(isInside);
    }

    public override bool ClickableCondition()
    {
        return base.ClickableCondition() && keyInLock;
    }
}
