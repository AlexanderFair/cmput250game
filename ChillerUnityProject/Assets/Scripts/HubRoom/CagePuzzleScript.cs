using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    //The disabled sprite for the unlock button
    public Sprite disabledButtonSprite;
    //The enabled sprite for the unlock button
    public Sprite enabledButtonSprite;
    //The clicked sprite for the unlock button
    public Sprite clickedButtonSprite;

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
        UpdateClickableButtonSprite();
    }

    private void UpdateClickableButtonSprite()
    {
        if (CageRoomScript.unlocked)
        {
            spriteClickableOutlineRenderer.sprite = clickedButtonSprite;
        }else if(keyInLock)
        {
            spriteClickableOutlineRenderer.sprite = enabledButtonSprite;
        }
        else
        {
            spriteClickableOutlineRenderer.sprite = disabledButtonSprite;
        }
    }

    public void KeyInsideLock(bool isInside)
    {
        keyInLock = isInside;
        UpdateClickableButtonSprite();
        
    }

    public override bool ClickableCondition()
    {
        return base.ClickableCondition() && keyInLock;
    }
}
