using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Video;

/*
 * This is attatched to a video player object
 * The management of a cutscene is done by such class
 */
public class Cutscene : UIObjectClass {
    private const bool SHOULD_LOG_INFO = true; 
    // identifier for a cutscene
    public enum CutsceneID {
        INTRO
    }
    // the enum that stores current cutscene state
    public enum PlayPhase {
        LOADING, PENDING_START, PLAYING, FINISHED
    }
    

    [Header("Cutscene Basic Settings")]
    // this is assigned in the inspector
    public VideoPlayer attatchedCutscenePlayer;
    // which file is going to be played?
    public string nameFileToPlay = "";
    // what scene to switch to after finishing playing the Cutscene?
    public string sceneOnFinish = "";
    public Vector3 posOnFinish = Vector3.zero;
    [Header("Cutscene Identifier")]
    public CutsceneID cutsceneIdentifier;



    // internal variables
    // this flag is here because the file might not be loaded when start is requested
    protected bool _shouldStart = false,
    // if the cutscene is paused because a menu is open etc. 
    _isPaused = false,
    // prevent excessive triggers on finishPlayingCutscene()
    _finishTriggered = false;
    // saves current playing phase (loading, pre-start, playing, ended)
    protected PlayPhase _playingPhase = PlayPhase.LOADING;
    public  PlayPhase playingPhase {
        get {
            return _playingPhase;
        }
    }
    // internal dictionary that stores cutscenes
    private static Dictionary<CutsceneID, Cutscene> _cutsceneMap = new Dictionary<CutsceneID, Cutscene>();


    /*
     * internal use: caches THIS CURRENT cutscene based on its identifier; recommended to have a single call on awake. 
     * returns false if duplication is found (hence is the component destroyed at once).
     */
    protected bool cacheCutscene() {
        if (_cutsceneMap.ContainsKey(cutsceneIdentifier) ) {
            Settings.DisplayWarning("WARNING: possible duplication for cutscene " + cutsceneIdentifier + ". Do your cutscene have a wrong identifier?", gameObject);
            DestroyImmediate(gameObject);
            return false;
        }
        _cutsceneMap.Add(cutsceneIdentifier, this);
        return true;
    }
    /*
     * gets the cutscene cached with provided identifier.
     */
    public static Cutscene getCutsceneByID(CutsceneID identifier) {
        if (_cutsceneMap.ContainsKey(identifier) ) {
            return _cutsceneMap[identifier];
        }
        return null;
    }
    // hide when completed preparing
    void afterCompletion(VideoPlayer cutscenePlayer) {
        // if (SHOULD_LOG_INFO)
        //     Debug.Log("Hiding Cutscene so it does not hide the title screen");
        // IT SHOULD NOT COVER UP THE TITLE SCREEN!
        // cutscenePlayer.enabled = false;

    }
    // overwrites
    protected override void StartUIObject() {
        if ( cacheCutscene() ) {
            // generate url
            attatchedCutscenePlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, nameFileToPlay);
            // prepare
            attatchedCutscenePlayer.prepareCompleted += afterCompletion;
            attatchedCutscenePlayer.Prepare();
        }
    }
    protected override void OnDestroyUIObject() {
        finishPlayingCutscene();
    }
    protected override void UpdateUIObject() {
    }

    // Update is called once per frame
    void Update() {
        // play when the menu is not open
        if(!MenuObjectClass.IsMenuActive()) {
            switch (_playingPhase) {
                // check if the loading is done
                case PlayPhase.LOADING:
                    if (attatchedCutscenePlayer.isPrepared) {
                        if (SHOULD_LOG_INFO)
                            Settings.DisplayWarning("Cutscene " + cutsceneIdentifier + " is prepared and ready to launch", gameObject);
                        _playingPhase = PlayPhase.PENDING_START;
                    }
                    break;
                // if the should start flag is true, start playing
                case PlayPhase.PENDING_START:
                    if (_shouldStart) {
                        attatchedCutscenePlayer.Play();
                        _playingPhase = PlayPhase.PLAYING;
                        if (SHOULD_LOG_INFO)
                            Settings.DisplayWarning("Attempting to start playing Cutscene " + cutsceneIdentifier, gameObject);
                    }
                    break;
                // regularly check if the cutscene is finished playing
                case PlayPhase.PLAYING:
                    if (_isPaused) {
                        _isPaused = false;
                        attatchedCutscenePlayer.renderMode = VideoRenderMode.CameraNearPlane;
                        attatchedCutscenePlayer.Play();
                    }
                    if ((! attatchedCutscenePlayer.isPlaying) ) {
                        finishPlayingCutscene();
                    }
                    break;
            }
        }
        // pause when menu is open
        else {
            _isPaused = true;
            attatchedCutscenePlayer.renderMode = VideoRenderMode.CameraFarPlane;
            attatchedCutscenePlayer.Pause();
        }
    }

    /*
     * This is called when the cutscene should start playing
     */
    public void startPlayingCutscene() {
        attatchedCutscenePlayer.renderMode = VideoRenderMode.CameraNearPlane;
        _shouldStart = true;
        // pauses musics while playing cutscene
        AudioHandler.Instance.pauseAmbient();
        AudioHandler.Instance.pauseSoundtrack();
    }
    /*
     * This is called when the cutscene stops playing
     */
    public void finishPlayingCutscene() {
        if (_finishTriggered)
            return;
        _finishTriggered = true;
        _playingPhase = PlayPhase.FINISHED;
        // unpause the sounds
        AudioHandler.Instance.unpauseAmbient();
        AudioHandler.Instance.unpauseSoundtrack();
        // destroy the UI
        UIObjectClass.DestroyUIObject(gameObject);
        // switch scene, if configured
        if (sceneOnFinish != "") {
            GameManager.Instance.StartSwitchScene(sceneOnFinish, posOnFinish);
        }
        CutSceneBlackBackground.Instance?.DisableBackground();
        if (SHOULD_LOG_INFO)
            Settings.DisplayWarning("Cutscene " + cutsceneIdentifier + " was finished.", gameObject);
    }
}
