using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowbarUIScript : CombinationUIObject
{
    [Header("Crowbar UI Settings")]
    //The index of the solution gameobjectwithin their respective sets
    public int[] solution;
    //The submit button
    public CrowbarSubmitCodeButton submitButton;
    //The sound effect when the code is correct
    public AudioClip correctSound;
    //Spans the object if correct
    public GameObject correctObject;
    //The sound effect when the code is wrong
    public AudioClip wrongSound;
    //Spawns the object if false;
    public GameObject wrongObject;

    private CrowbarRoomScript room;

    //Called by a button wanting to submit the code
    public void SubmitCode()
    {
        if(TestIsSolved())
        {
            AudioHandler.Instance.playSoundEffect(correctSound);
            if(correctObject != null) { InstantiateUIElement(correctObject); }
            room.Solved();
        }
        else
        {
            AudioHandler.Instance.playSoundEffect(wrongSound);
            if (wrongObject != null) { InstantiateUIElement(wrongObject); }
        }
    }

    private bool TestIsSolved()
    {
        for (int i = 0; i < solution.Length; i++)
        {
            if (solution[i] != values[i])
            {
                return false;
            }
        }
        return true;
    }

    //Called by the room object when created
    public void Setup(CrowbarRoomScript _room){
        room = _room;
    }

}
