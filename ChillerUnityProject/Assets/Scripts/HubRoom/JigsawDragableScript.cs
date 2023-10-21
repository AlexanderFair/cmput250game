using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The piece
public class JigsawDragableScript : SnapDragUIObject
{
    [Header("Jigsaw Piece Settings")]
    //The puzzle
    public JigsawPuzzleScript puzzle;

    public override bool ClickableCondition()
    {
        return base.ClickableCondition() && !puzzle.IsPuzzleComplete();
    }
}
