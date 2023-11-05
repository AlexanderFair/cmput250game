using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboUI : CombinationUIObject
{
    private ComboPuzzle puzz;
    [Header("ComoUI")]
    public ComboUISubmitBtn submitBtn;

    public void Setup(ComboPuzzle puzz)
    {
        this.puzz = puzz;
    }

    public void SubmitCode()
    {
        puzz.CodeUpdated(values);
    }
        
}
