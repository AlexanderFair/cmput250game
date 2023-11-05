using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioUISubmitBtn : ClickableUIObject
{
    [Header("Submit Button")]
    public RadioPuzzle puzzle;

    protected override void Clicked()
    {
        base.Clicked();
        puzzle.SubmitFrequency();
    }

}
