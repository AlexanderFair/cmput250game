using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowbarDrawerScript : ClickableUIObject
{
    [Header("Crowbar Drawer Settings")]
    public DialogDisplay.DialogStruct alreadyRetrievedPrompt;
    public DialogDisplay.DialogStruct retrievedPrompt;
    public DialogDisplay.DialogStruct presentPrompt;

    protected override void StartUIObject()
    {
        base.StartUIObject();
        if (CrowbarRoomScript.HasCrowbar)
        {
            DialogDisplay.NewDialog(alreadyRetrievedPrompt);
            spriteClickableOutlineRenderer.enabled = false;
        }
        else
        {
            DialogDisplay.NewDialog(presentPrompt);
        }
    }

    public override bool ClickableCondition()
    {
        return base.ClickableCondition() && !CrowbarRoomScript.HasCrowbar;
    }

    protected override void Clicked()
    {
        base.Clicked();
        CrowbarRoomScript.HasCrowbar = true;
        spriteClickableOutlineRenderer.enabled = false;
        DialogDisplay.NewDialog(retrievedPrompt);
    }
}
