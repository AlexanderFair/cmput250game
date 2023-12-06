using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoilerRoomEntrance : InteractableRoomObject
{
    [Header("Entrance Settings")]
    public DialogDisplay.DialogStruct[] incompletePrompt;
    public string nextScene;
    public Vector3 nextLocation;

    public override void Start()
    {
        base.Start();
        if (!GameCompletionManager.BoilerRoomComplete)
        {
            DialogDisplay.NewDialog(incompletePrompt);
        }

    }

    protected override void Interact()
    {
        base.Interact();
        GameManager.Instance.StartSwitchScene(nextScene, nextLocation);
    }
}

//The weather can become frigid cold in the evenings, even for the magestic penguin.