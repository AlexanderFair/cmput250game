using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioUI : UIObjectClass
{
    [Header("Radio UI")]
    public RadioUIDialObject dialX;
    public RadioUIDialObject dialY;
    
    private RadioPuzzle puzzle;

    public float waitForTime = 0.5f;
    private float time = 0;
    private bool check = false;

    public void CreatedRadioUI(RadioPuzzle p)
    {
        puzzle = p;
    }

    private int preX = 0, preY = 0;

    protected override void UpdateUIObject()
    {
        base.UpdateUIObject();
        time += Time.deltaTime;
        if(dialX.Value != preX || dialY.Value != preY)
        {
            preX = dialX.Value; preY = dialY.Value;
            Change();
        }

        if(time > waitForTime && !check)
        {
            puzzle.UpdatePuzzle(preX, preY);
            check = true;
        }
    }

    private void Change()
    {
        time = 0;
        check = false;
    }



}
