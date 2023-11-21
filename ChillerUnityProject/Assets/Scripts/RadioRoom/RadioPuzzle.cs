using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioPuzzle : UIObjectClass
{
    [Header("Radio Puzzle")]
    public RadioUIDialObject dialX;
    public RadioUIDialObject dialY;
    public RadioUISubmitBtn submitBtn;
    
    
    private RadioRoomObject roomObj;
    private int prevCompID = -1;

    public void SetUp(RadioRoomObject _roomObj, int previouslyCompletedComboID)
    {
        roomObj = _roomObj;
        if (roomObj.IsAllComplete())
        {
            //all combos are already complete
            DialogDisplay.NewDialog(GameCompletionManager.BoilerRoomComplete ? roomObj.alreadyCalledForHelpBoilerComplete : roomObj.alreadyCalledForHelpBoilerNotComplete);
            prevCompID = previouslyCompletedComboID;
        }
        else
        {
            roomObj.PromptNextCombo();
        }
    }

    public void SubmitFrequency()
    {
        if(prevCompID >= 0)
        {
            PrevCompleteSubmitFrequency();
            return;
        }

        for(int i=0; i < roomObj.combos.Length; i++)
        {
            Vector2Int combo = roomObj.combos[i];
            if(dialX.Value == combo.x && dialY.Value == combo.y)
            {
                CompleteCombo(i);
                return;
            }
        }

        //not one of the completion combos

        if (TestSpecialCombos())
        {
            //one of the special combos worked
            return;
        }

        DialogDisplay.NewDialog(roomObj.badChoice);
        AudioHandler.Instance.playSoundEffect(Util.ChooseRandom(roomObj.badChoiceEffects));
        roomObj.badInsanityAdd.Add();
    }

    private void PrevCompleteSubmitFrequency()
    {
        if(dialX.Value == roomObj.combos[prevCompID].x && dialX.Value == roomObj.combos[prevCompID].y)
        {
            AudioHandler.Instance.playSoundEffect(Util.ChooseRandom(roomObj.alreadyCalledForHelpSubmitEffects));
            DialogDisplay.NewDialog(roomObj.alreadyCalledForHelpSubmit);
            return;
        }
        if (TestSpecialCombos())
        {
            //special Comobo worked
            return;
        }
        AudioHandler.Instance.playSoundEffect(Util.ChooseRandom(roomObj.alreadyCalledForHelpSubmitEffects));
        DialogDisplay.NewDialog(roomObj.alreadyCalledForHelpSubmit);
    }

    private bool TestSpecialCombos()
    {
        for (int i = 0; i < roomObj.specialCombos.Length; i++)
        {
            Vector2Int combo = roomObj.specialCombos[i];
            if (dialX.Value == combo.x && dialY.Value == combo.y)
            {
                AudioHandler.Instance.playSoundEffect(Util.ChooseRandom(roomObj.loreEffects));
                DialogDisplay.NewDialog(roomObj.lore[i]);
                roomObj.loreInsanityAdd.Add();
                return true;
            }
        }
        return false;
    }
    
    private void CompleteCombo(int id)
    {
        if (roomObj.CompleteFrequency(id))
        {
            //it has been completed for the first time
            if(roomObj.IsAllComplete())
            {
                //puzle is complete, let the room object take over
                roomObj.CompletePuzzle(id);
                prevCompID = id;
                return;
            }

            //TODO prompt to do next combo
            AudioHandler.Instance.playSoundEffect(Util.ChooseRandom(roomObj.goodCodeEffects));
            roomObj.PromptNextCombo(roomObj.completeCodePrompts[RadioRoomObject.completedFreqs]);
        }
        else
        {
            // we have already completed this code
            AudioHandler.Instance.playSoundEffect(Util.ChooseRandom(roomObj.badChoiceEffects));
            DialogDisplay.NewDialog(roomObj.alredayChosen);
        }
    }
    

    



}
