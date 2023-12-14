using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboPuzzle : DisplayUIRoomObject
{
    [Header("Combo Puzzle")]
    //The solution to the puzzle based off of indexs of the combination elements in the UI
    public int[] solve;
    //Played when the puzzle is open in the incomplete form
    public DialogDisplay.DialogStruct[] incompletePrompt;
    //Played when the puzzle is completed
    public DialogDisplay.DialogStruct completePrompt;
    //Played when the code is subitted and is incorrect
    public DialogDisplay.DialogStruct[] wrongCodePrompt;
    public Insanity.AddAmount wrongCodeInsanity;

    //Played when the puzzle is completed
    public AudioClip unlockAudio;
    //Played when the wrong code is submitted
    public AudioClip[] wrongComboEffect;
    //Displayed when the puzzle is complete
    public GameObject drawerUi;

    public Sprite unlockedSprite;
    public Sprite lockedSprite;

    private int[] lastUpdatedCode = null;

    private static bool complete = false;

    public override void Start()
    {
        base.Start();
        interactableRenderer.sprite = complete ? unlockedSprite : lockedSprite;
    }
    protected override void DisplayedUI()
    {
        base.DisplayedUI();
        if (complete)
        {
            UIObjectClass.DestroyUIObject(ui);
            UIObjectClass.InstantiateNewUIElement(drawerUi);
        }
        else
        {
            ui.GetComponent<ComboUI>().Setup(this, lastUpdatedCode);
            DialogDisplay.NewDialog(incompletePrompt);
        }
    }
    
    public void CodeUpdated(int[] code)
    {
        lastUpdatedCode = (int[])code.Clone();
    }

    public void CodeSubmitted(int[] code)
    {
        CodeUpdated(code);
        bool yes = true;
        for(int i=0; i<solve.Length; i++)
        {
            yes &= solve[i] == code[i];
        }
        if (!yes)
        {
            AudioHandler.Instance.playSoundEffect(Util.ChooseRandom(wrongComboEffect));
            DialogDisplay.NewDialog(wrongCodePrompt);
            wrongCodeInsanity.Add();
            return;
        }
        complete = true;
        interactableRenderer.sprite = unlockedSprite;
        AudioHandler.Instance.playSoundEffect(unlockAudio);
        UIObjectClass.InstantiateNewUIElement(drawerUi);
        DialogDisplay.NewDialog(completePrompt);
    }

}
