using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioPuzzle : DisplayUIRoomObject
{
    [Header("Radio Puzzle")]
    public string radioPromptSnap;
    public string radioPromptCombo;
    public string completionNoteSnap;
    public string completionNoteCombo;
    public string alreadyComplete;
    public string allComplete;
    public string tryAgain;

    public Vector2Int snapValue;
    public Vector2Int comboValue;

    private bool snap = false, combo = false;

    public void UpdatePuzzle(int xval, int yval)
    {
        if (xval == snapValue.x && yval == snapValue.y)
        {
            if (snap)
            {
                DialogDisplay.NewDialog(alreadyComplete, AnimationSpriteClass.NULL_STRUCT);
            }
            else
            {
                snap = true;
                DialogDisplay.NewDialog(completionNoteSnap + (combo ? "" : tryAgain), AnimationSpriteClass.NULL_STRUCT);
            }
        }

        if (xval == comboValue.x && yval == comboValue.y)
        {
            if (combo)
            {
                DialogDisplay.NewDialog(alreadyComplete, AnimationSpriteClass.NULL_STRUCT);
            }
            else
            {
                combo = true;
                DialogDisplay.NewDialog(completionNoteCombo + (snap ? "" : tryAgain), AnimationSpriteClass.NULL_STRUCT);
            }
        }
    }


    protected override void DisplayedUI()
    {
        base.DisplayedUI();
        if (!snap)
        {
            DialogDisplay.NewDialog(radioPromptSnap, AnimationSpriteClass.NULL_STRUCT);
        }else if (!combo)
        {
            DialogDisplay.NewDialog(radioPromptCombo, AnimationSpriteClass.NULL_STRUCT);
        }
        else
        {
            DialogDisplay.NewDialog(allComplete, AnimationSpriteClass.NULL_STRUCT);
        }

        ui.GetComponent<RadioUI>().CreatedRadioUI(this);
    }

}
