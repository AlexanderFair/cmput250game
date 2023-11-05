using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboUISubmitBtn : ClickableUIObject
{
    [Header("Submit Button")]
    public ComboUI puzzle;

    protected override void Clicked()
    {
        base.Clicked();
        puzzle.SubmitCode();
    }

}