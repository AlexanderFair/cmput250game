using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleRoomObj : DisplayUIRoomObject
{
    public PipeGrid.PipePuzzles levelName;
    public int liquidAmount = 100;
    public bool isCompleted = false;

    public DialogDisplay.DialogStruct[] lockedPrompt;
    public DialogDisplay.DialogStruct[] unlockedPrompt;

    // when re-entering the room, synchronize the puzzle solve state
    public void Awake() {
        isCompleted = PipeGrid.getPuzzle(levelName).isSolved();
    }

    public override bool InteractableCondition() {
        // do not interact after this is finished
        return base.InteractableCondition() && !isCompleted;
    }

    protected override void Interact()
    {
        bool locked = false;

        if (levelName == PipeGrid.PipePuzzles.HARD)
        {
            if (!(PipeGrid.getPuzzle(PipeGrid.PipePuzzles.INTRO).isSolved() &&
                    PipeGrid.getPuzzle(PipeGrid.PipePuzzles.MEDIUM).isSolved()))
            {
                locked = true;
            }
        }

        DialogDisplay.NewDialog(locked? lockedPrompt : unlockedPrompt);

        if (!locked)
        {
            base.Interact();
        }

    }
    // call this to re-open UI
    public void ReopenUI()
    {
        // close the UI 
        UIObjectClass.ClearUI();
        // re-open
        Interact();
    }
    // called after the UI is displayed
    protected override void DisplayedUI()
    {
        PipeGrid.setPuzzle(levelName, this, ui);
    }
}
