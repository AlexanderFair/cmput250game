using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowbarDrawerScript : ClickableUIObject
{
    [Header("Crowbar Drawer Settings")]
    public string alreadyRetrievedPrompt;
    public string retrievedPrompt;
    public string presentPrompt;


    protected override void AwakeUIObject()
    {
        base.AwakeUIObject();
        if (CrowbarRoomScript.HasCrowbar)
        {
            DialogDisplay.NewDialog(alreadyRetrievedPrompt, AnimationSpriteClass.NULL_STRUCT);
            spriteClickableOutlineRenderer.enabled = false;
        }
        else
        {
            DialogDisplay.NewDialog(presentPrompt, AnimationSpriteClass.NULL_STRUCT);
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
        DialogDisplay.NewDialog(retrievedPrompt, AnimationSpriteClass.NULL_STRUCT);
    }
}
