using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleRoomObj : DisplayUIRoomObject
{
    public String levelName;
    bool isCompleted = false;

    // Update is called once per frame
    protected override void UpdateRoomObject()
    {
        base.UpdateRoomObject();

        if (Condition())
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
        
    }
    // do not interact after this is finished
    protected override bool Condition() {
        return Interactable && !isCompleted;
    }
    // called after the UI is displayed
    protected override void DisplayedUI()
    {
        PipeGrid.setPuzzle(levelName, this, ui);
    }
}
