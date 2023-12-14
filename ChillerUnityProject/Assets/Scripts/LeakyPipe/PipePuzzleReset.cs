using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipePuzzleReset : ClickableUIObject
{
    protected override void Clicked() {
        base.Clicked();
        // validate if the puzzle is solved properly; no reset for a finished puzzle.
        if (PipeGrid.getPuzzle().isSolved()) {
            return;
        }
        // use the original Room Obj that opened this puzzle to reopen UI
        PipeGrid.triggeredRoomObj.ReopenUI();
    }
}
