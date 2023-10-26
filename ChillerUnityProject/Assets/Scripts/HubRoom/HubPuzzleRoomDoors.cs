using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubPuzzleRoomDoors : DisableInteractableRoomObject
{
    [Header("Change Scene Settings")]
    public String nextSceneName = "";
    public Vector3 playerStartPosition;
    // Dialogs that will be randomly choose from when the player interacts with the door when its locked
    public string[] lockedDialogs;
    public AudioClip[] lockedEffects = new AudioClip[0];
    //indexes of clips in the settingsInstance.audioClips
    public int[] unlockedEffectsSettingsIndex = new int[0];

    private System.Random random = new System.Random();

    public override void Start()
    {
        base.Start();
        if (nextSceneName == "")
        {
            Settings.DisplayError("A scene name was not provided for the scene changer", gameObject);
        }
        if (playerStartPosition == Vector3.zero)
        {
            Settings.DisplayWarning("The selected startposition is zero", gameObject);
        }

        if (CrowbarRoomScript.HasCrowbar)
        {
            DisableInteract();
        }
    }

    protected override void UpdateRoomObject()
    {
        base.UpdateRoomObject();
        if(!disabledInteract && CrowbarRoomScript.HasCrowbar)
        {
            DisableInteract();
        }
    }

    protected override void Interact()
    {
        base.Interact();
        if(disabledInteract)
        {
            ChangeScenes();
            if (unlockedEffectsSettingsIndex.Length > 0)
            {
                AudioHandler.Instance.playSoundEffect(SettingsInstance.Instance.audioClips[unlockedEffectsSettingsIndex[Settings.randomInstance.Next(unlockedEffectsSettingsIndex.Length)]]);
            }
        }
        else
        {
            if (lockedEffects.Length > 0)
            {
                AudioHandler.Instance.playSoundEffect(lockedEffects[Settings.randomInstance.Next(lockedEffects.Length)]);
            }
            StopPlayer();
        }
    }

    private void ChangeScenes()
    {
        Settings.DisplayWarning("Changing scenes to " + nextSceneName, gameObject);
        if (nextSceneName == "")
        {
            Settings.DisplayError("You did not specify the next scene for the ChangeSceneRoomObject!", gameObject);
            return;
        }

        GameManager.Instance.StartSwitchScene(nextSceneName, playerStartPosition);
    }

    private void StopPlayer()
    {
        if(lockedDialogs.Length > 0)
        {
            DialogDisplay.NewDialog(lockedDialogs[random.Next(lockedDialogs.Length)], AnimationSpriteClass.NULL_STRUCT);
        }
    }
}
