using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboUI : CombinationUIObject
{
    private ComboPuzzle puzz;

    public void Setup(ComboPuzzle puzz)
    {
        this.puzz = puzz;
    }

    protected override void CodeUpdated()
    {
        base.CodeUpdated();
        puzz.CodeUpdated(values);
        
    }
}
