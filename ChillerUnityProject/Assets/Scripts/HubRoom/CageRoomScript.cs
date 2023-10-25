using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The cage that holds the penguin
// This uses a static variable for unlock, so there should not be more than one instance of this
public class CageRoomScript : DisableInteractableRoomObject
{
    [Header("Cage Objects")]
    //the penguin object
    public Penguin penguin;
    //the position where the penguin should be when locked in th ecage
    public Vector3 penguinPositionOnLock;
    //the position where the penguin should transport to when unlocked
    public Vector3 penguinPositionOnUnlock;

    public string notUnlockedPrompt;

    private static int instanceExists = 0;
    public static bool unlocked = false;

    public override void Start()
    {
        base.Start();
        if (instanceExists > 0)
        {
            Settings.DisplayWarning("An instance of cage already exists", gameObject);
        }
        instanceExists++;
        if (unlocked)
        {
            StartUnlocked();
        }
        else
        {
            StartLocked();
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        instanceExists--;
    }

    //called when the cage is initialized in the unlock state
    private void StartLocked()
    {
        penguin.SetLockedInCage(true);
        penguin.transform.position = penguinPositionOnLock;
    }

    // called when the cage is initialized in the lock state
    private void StartUnlocked()
    {
        UnlockCage();
        //TODO set penguin location
    }

    // Sets the puzzle to unlocked an dreleases the penguin
    public void UnlockCage()
    {
        penguin.SetLockedInCage(false);
        unlocked = true;
        penguin.transform.position = penguinPositionOnUnlock;
        DisableInteract();
    }

    protected override void Interact()
    {
        base.Interact();
        if(!CrowbarRoomScript.HasCrowbar)
        {
            DialogDisplay.NewDialog(notUnlockedPrompt, AnimationSpriteClass.NULL_STRUCT);
        }
        else
        {
            UnlockCage();
        }
    }


}
