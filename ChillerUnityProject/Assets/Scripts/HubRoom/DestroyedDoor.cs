using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedDoor : InteractableRoomObject
{
    [Header("Destoryed Door")]
    public DialogDisplay.DialogStruct[] lowInsanity;
    public DialogDisplay.DialogStruct[] highInsanity;
    public Insanity.AddAmount lowInsanityAdd;
    public Insanity.AddAmount highInsanityAdd;

    protected override void Interact()
    {
        DialogDisplay.NewDialog(Insanity.Instance.IsHigh() ? highInsanity : lowInsanity);
        base.Interact();
        (Insanity.Instance.IsHigh() ? highInsanityAdd : lowInsanityAdd).Add();
    }
}
