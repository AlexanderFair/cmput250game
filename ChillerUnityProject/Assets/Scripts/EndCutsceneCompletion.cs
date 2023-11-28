using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCutsceneCompletion : CutsceneConclusion
{
    public void Start()
    {
        MenuController.Instance.inCutscene = true;

        AudioHandler.Instance.trackVolume = 0;
        AudioHandler.Instance.effectVolume = 0;
        AudioHandler.Instance.ambientVolume = 0;
        AudioHandler.Instance.SetVolumes();
    }
    public override void OnCutsceneEnd()
    {
        Debug.Log("Complete");
        Application.Quit();

        AudioHandler.Instance.pauseAmbient();
        AudioHandler.Instance.pauseSoundtrack();
    }
}
