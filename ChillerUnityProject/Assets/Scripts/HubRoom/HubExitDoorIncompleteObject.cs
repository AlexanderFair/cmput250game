using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubExitDoorIncompleteObject : InteractableRoomObject
{
    [Header("Exit Door Settings")]
    public List<DialogDisplay.DialogStruct> radioIncompletePrompts;
    public List<DialogDisplay.DialogStruct> boilerIncompletePrompts;
    public List<DialogDisplay.DialogStruct> bothIncompletePrompts;
    public bool addRadioAndBoilerIncompleteToBoth = true;
    public List<DialogDisplay.DialogStruct> hubIncompletePrompts;

    public override void Start()
    {
        base.Start();
        if (addRadioAndBoilerIncompleteToBoth)
        {
            bothIncompletePrompts.AddRange(radioIncompletePrompts);
            bothIncompletePrompts.AddRange(boilerIncompletePrompts);
        }
    }

    protected override void Interact()
    {
        base.Interact();
        if (!GameCompletionManager.RadioRoomComplete && !GameCompletionManager.BoilerRoomComplete)
        {
            DialogDisplay.NewDialog(Util.ChooseRandom(bothIncompletePrompts));
        }
        else if (!GameCompletionManager.RadioRoomComplete)
        {
            DialogDisplay.NewDialog(Util.ChooseRandom(radioIncompletePrompts));
        }
        else if (!GameCompletionManager.BoilerRoomComplete)
        {
            DialogDisplay.NewDialog(Util.ChooseRandom(boilerIncompletePrompts));
        }
        else if (!GameCompletionManager.HubRoomComplete)
        {
            DialogDisplay.NewDialog(Util.ChooseRandom(hubIncompletePrompts));
        }
    }
}
