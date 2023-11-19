using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioRoomObject : DisplayUIRoomObject
{
    [Header("Radio Puzzle")]
    public Vector2Int[] combos;
    //Only change this if there are multiple radios in the game
    public int bitComboStorageOffset = 0;
    //Combos for lore notes
    public Vector2Int[] specialCombos;

    [Header("Radio Puzzle Prompts")]
    // Played when the puzzle is completed
    public DialogDisplay.DialogStruct contactedHelpBoilerNotComplete;
    // Played when the puzzle is completed
    public DialogDisplay.DialogStruct contactedHelpBoilerComplete;
    // Played when the radio is open after help has already been called
    public DialogDisplay.DialogStruct[] alreadyCalledForHelpBoilerNotComplete;
    public DialogDisplay.DialogStruct[] alreadyCalledForHelpBoilerComplete;
    //Played when the player tries to submit a code when help is already called
    public DialogDisplay.DialogStruct[] alreadyCalledForHelpSubmit;
    //Played whenever the submit button is clicked when the puzzle has already been completed unless the codes match a specialCombo
    public AudioClip[] alreadyCalledForHelpSubmitEffects;
    //Played when a bad frequency is choosen
    public DialogDisplay.DialogStruct[] badChoice;
    //Played whenever the submit button and the combo is wrong
    public AudioClip[] badChoiceEffects;

    public Insanity.AddAmount badInsanityAdd;
    //Played when the frequency has already been called
    public DialogDisplay.DialogStruct[] alredayChosen;
    //Played to hint at the next combo
    //Played when the radio is first opened and not already complete or when a combo is selected and theres a next one
    //The hint is choosen randomly from the lowest ids which has not been completed
    public Prompts[] promptsForNext;
    //Added to the beginning of the next combo hint
    //Played when a good combo is submited which is not the last combo
    public string[] completeCodePrompts;
    //Played when a good combo is selected for the first time but is not the final combo
    public AudioClip[] goodCodeEffects;
    //Payed when a lore code is selected
    public DialogDisplay.DialogStruct[] lore;
    //Played when a lore code is selected
    public AudioClip[] loreEffects;
    public Insanity.AddAmount loreInsanityAdd;
    
    


    //Id is set to 1 if that id was completed
    public static ulong combosComplete = 0ul;
    //Set to the id if the combo was completed using that id
    //set to -1 otherwise
    public static Dictionary<int, int> completionIds = new Dictionary<int, int>();

    private ulong comboMask;
    public override void Start()
    {
        base.Start();
        comboMask = ((1ul << combos.Length) - 1) <<  bitComboStorageOffset;
        if(!completionIds.ContainsKey(bitComboStorageOffset)) 
        {
            completionIds[bitComboStorageOffset] = -1;
        }
    }

    public bool IsFrequencyComplete(int id)
    {
        return (combosComplete & (1ul << (id + bitComboStorageOffset))) != 0;
    }

    // Retuns true if the frequency was not complete before
    public bool CompleteFrequency(int id)
    {
        bool b = IsFrequencyComplete(id);
        combosComplete |= (1ul << (id + bitComboStorageOffset));
        return !b;
    }

    //Returns true if all combos are complete
    public bool IsAllComplete()
    {
        return (combosComplete & comboMask) == comboMask;
    }

    public void CompletePuzzle(int id)
    {
        //called when the puzzle was completed with the id
        completionIds[bitComboStorageOffset] = id;
        UIObjectClass.ClearUI();

        DialogDisplay.NewDialog(GameCompletionManager.BoilerRoomComplete ? contactedHelpBoilerComplete : contactedHelpBoilerNotComplete);
        GameCompletionManager.RadioRoomComplete = true;
    }



    protected override void DisplayedUI()
    {
        base.DisplayedUI();
        ui.GetComponent<RadioPuzzle>().SetUp(this, completionIds[bitComboStorageOffset]);
    }


    [System.Serializable]
    public struct Prompts
    {
        public DialogDisplay.DialogStruct[] prompts;
    }

    //Chooses a random prompt from the union of lists of prompts of ids whihc have not been completed
    public void PromptNextCombo(string addedTextToFront = "", bool chooseFromLowest = true)
    {
        List<DialogDisplay.DialogStruct[]> canPrompt = new List<DialogDisplay.DialogStruct[]>();
        for(int i=0; i < promptsForNext.Length; i++)
        {
            if (!IsFrequencyComplete(i))
            {
                canPrompt.Add(promptsForNext[i].prompts);
                if (chooseFromLowest)
                {
                    break;
                }
            }
        }
        DialogDisplay.DialogStruct p = Util.ChooseRandom(Util.ChooseRandom(canPrompt));
        DialogDisplay.NewDialog(addedTextToFront + p.dialog, p.animation);
    }

}
