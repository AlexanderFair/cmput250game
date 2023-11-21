using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCutsceneCompletion : CutsceneConclusion
{
    public void Start()
    {
        MenuController.Instance.inCutscene = true;
    }
    public override void OnCutsceneEnd()
    {
        Debug.Log("Complete");
        Application.Quit();
        AudioHandler.Instance.pauseAmbient();
        AudioHandler.Instance.pauseSoundtrack();
    }
}
