using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioRoomEntrance : InteractableRoomObject
{
    [Header("Entrance Settings")]
    public DialogDisplay.DialogStruct entrancePrompt;
    public DialogDisplay.DialogStruct[] incompletePrompt;
    public DialogDisplay.DialogStruct[] completebincompletePrompt;
    public DialogDisplay.DialogStruct[] completebcompletePrompt;
    public string nextScene;
    public Vector3 nextLocation;

    public override void Start()
    {
        base.Start();
        DialogDisplay.NewDialog(entrancePrompt);

    }

    protected override void Interact()
    {
        base.Interact();
        GameManager.Instance.StartSwitchScene(nextScene, nextLocation);
    }
}

//The weather can become frigid cold in the evenings, even for the magestic penguin.