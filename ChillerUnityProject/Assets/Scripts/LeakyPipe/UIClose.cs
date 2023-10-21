using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIClose : ClickableUIObject
{
    protected override void Clicked() {
        base.Clicked();
        // validate if the puzzle is solved properly
        if (PipeGrid.getPuzzle().isSolved()) {
            PipeGrid.triggeredRoomObj.isCompleted = true;
        }
        // close the UI 
        ClearUI();
    }
}
