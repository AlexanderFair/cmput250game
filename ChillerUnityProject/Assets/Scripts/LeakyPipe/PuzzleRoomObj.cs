﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleRoomObj : DisplayUIRoomObject
{
    public String levelName;
    public bool isCompleted = false;

    // do not interact after this is finished
    public override bool InteractableCondition() {
        return base.InteractableCondition() && !isCompleted;
    }
    // called after the UI is displayed
    protected override void DisplayedUI()
    {
        PipeGrid.setPuzzle(levelName, this, ui);
    }
}