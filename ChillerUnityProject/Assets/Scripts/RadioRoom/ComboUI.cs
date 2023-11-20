using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboUI : CombinationUIObject
{
    private ComboPuzzle puzz;
    [Header("ComoUI")]
    public ComboUISubmitBtn submitBtn;

    public void Setup(ComboPuzzle puzz, int[] code)
    {
        this.puzz = puzz;
        defaultValues = code;
    }

    public void SubmitCode()
    {
        puzz.CodeUpdated(values);
    }
        
}
