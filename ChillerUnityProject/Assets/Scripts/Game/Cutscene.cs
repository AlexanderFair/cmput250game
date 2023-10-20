using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Cutscene : MonoBehaviour
{
    // Start is called before the first frame update

    public PlayableDirector playableDirector;
    public Canvas canvas;
    void Start()
    {
        playableDirector.Play();

        playableDirector.stopped += OnStopPlayingCutscene;
        //playableDirector.time = 5.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(playableDirector.playableAsset.duration + ", " + playableDirector.time);
        // just in case it does not stop
        if (playableDirector.time > playableDirector.playableAsset.duration) {
        // if (playableDirector.time > 5) {
            playableDirector.Stop();
        }
        
    }


    void OnStopPlayingCutscene(PlayableDirector plDir) {
        if (plDir == playableDirector) {
            Debug.Log("o????");
            GameManager.Instance.StartSwitchScene("HubRoom", Vector3.zero);
            Debug.Log("o!!!!");
        }
    }
}
