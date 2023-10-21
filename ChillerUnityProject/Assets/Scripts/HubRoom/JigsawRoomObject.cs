using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The super class for the room object which implements the jigsaw puzzle
public class JigsawRoomObject : DisplayUIRoomObject
{
    protected override void DisplayedUI()
    {
        base.DisplayedUI();
        ui.GetComponent<JigsawPuzzleScript>().Setup(this);
    }

    // Called when the jigsaw is solved
    public virtual void SetSolved() { }

    // True if the puzzle is solved
    public virtual bool IsSolved()
    {
        return false;
    }
}
