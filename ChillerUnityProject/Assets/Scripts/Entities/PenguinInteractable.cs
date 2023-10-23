using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinInteractable : InteractableRoomObject
{
    [Header("Penguin Interactable")]
    public Penguin penguin;

    public override bool InteractableCondition()
    {
        return base.InteractableCondition() && !penguin.Locked;
    }

    protected override void Interact()
    {
        base.Interact();
    }
}
