using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboPuzzle : DisplayUIRoomObject
{
    [Header("Combo Puzzle")]
    public int[] solve;
    public string incompletePrompt;
    public string completePrompt;
    public AudioClip unlockAudio;
    public GameObject noteUi;

    private bool complete = false;

    protected override void DisplayedUI()
    {
        base.DisplayedUI();
        if (complete)
        {
            UIObjectClass.DestroyUIObject(ui);
            UIObjectClass.InstantiateNewUIElement(noteUi);
        }
        else
        {
            ui.GetComponent<ComboUI>().Setup(this);
            DialogDisplay.NewDialog(incompletePrompt, AnimationSpriteClass.NULL_STRUCT);
        }
    }

    public void CodeUpdated(int[] code)
    {
        bool yes = true;
        for(int i=0; i<solve.Length; i++)
        {
            yes &= solve[i] == code[i];
        }
        if (!yes)
        {
            return;
        }
        complete = true;
        AudioHandler.Instance.playSoundEffect(unlockAudio);
        UIObjectClass.InstantiateNewUIElement(noteUi);
        DialogDisplay.NewDialog(completePrompt, AnimationSpriteClass.NULL_STRUCT);
    }

}
