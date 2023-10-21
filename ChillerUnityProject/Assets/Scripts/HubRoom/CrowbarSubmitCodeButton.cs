using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represents the sumbit button for the crow bar puzzle
public class CrowbarSubmitCodeButton : ClickableUIObject
{
    [Header("Crowbar Submit Button")]
    //The puzzle script
    public CrowbarUIScript ui;


    public override bool ClickableCondition()
    {
        return base.ClickableCondition() && !CrowbarRoomScript.Complete;
    }

    protected override void Clicked()
    {
        base.Clicked();
        ui.SubmitCode();
    }
}
