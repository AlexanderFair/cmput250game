using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleRoomObj : DisplayUIRoomObject
{
    public PipeGrid.PipePuzzles levelName;
    public int liquidAmount = 100;
    public bool isCompleted = false;

    public void Awake()
    {
        isCompleted = PipeGrid.getPuzzle(levelName).isSolved();
    }

    public override bool InteractableCondition() {
        // the final challenge has prerequisites!
        if (levelName == PipeGrid.PipePuzzles.HARD) {
            if ( !(PipeGrid.getPuzzle(PipeGrid.PipePuzzles.INTRO).isSolved() && 
                    PipeGrid.getPuzzle(PipeGrid.PipePuzzles.MEDIUM).isSolved()) ) {
                        dialog = "You trace the path of the snaking pipes throughout the room, ultimately finding yourself staring up at the metallic heart of the heating system. However, the boiler is cold to the touch: the machine has been cut off from its supply of hot water.";
                        return false;
                    }
            dialog = "I’m so close to getting the heat working.. Just this one last thing to fix.";
        }

        // do not interact after this is finished
        return base.InteractableCondition() && !isCompleted;
    }
    // called after the UI is displayed
    protected override void DisplayedUI()
    {
        PipeGrid.setPuzzle(levelName, this, ui);
    }
}
