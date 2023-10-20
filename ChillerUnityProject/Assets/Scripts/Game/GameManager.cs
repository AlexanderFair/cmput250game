using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Game Manager handles save information and room switching
 */
public class GameManager : MonoBehaviour
{

    // the room data storate. Make sure room objects update them on init based on those data!
    private static Dictionary<String, GameSaveInfo> _roomData;
    private static GameManager _instance;
    private static bool _instanceDefined = false;

    private float currentProgress = 0f;
    private bool switching = false;
    private bool complete = false;
    private Vector3 targetPos = Vector3.zero;

    public static GameManager Instance {
        get {
            if (!_instanceDefined)
                Debug.Log("Warning: no valid game manager instance is present. ");
            return _instance;
        }
    }

    // a game save for a room. you may create your own subclass of it if you find it necessary!
    public class GameSaveInfo {
        public static Dictionary<String, object> saveData = new Dictionary<String, object>();
    }

    // make sure when room switching, the player do not wiggle around.
    public void FixedUpdate()
    {


        if(switching && complete)
        {
            switching = false;
            complete = false;
            Player.Instance.getRigidBody().position = targetPos;
            targetPos = Vector3.zero;
            MenuObjectClass.DisableMenu();
            Settings.EnableInput();
        }
    }

    // this should not be destroyed when the scenes switch around.
    public void Awake()
    {
        if (_instanceDefined)
        {
            DestroyImmediate(gameObject);
            return;
        }
        _instance = this;
        _instanceDefined = true;
        _roomData = new Dictionary<String, GameSaveInfo>();


        DontDestroyOnLoad(gameObject);

    }

    // set a room save info for a given room
    public static void SetRoomSaveInfo(String RoomName, GameSaveInfo gameInfo) {
        _roomData[RoomName] = gameInfo;
    }
    // get a room save info. if not found, a default one is saved and returned.
    // you may specify the "default" value.
    public static GameSaveInfo GetRoomSaveInfo(String RoomName) {
        return GetRoomSaveInfo(RoomName, new GameSaveInfo());
    }
    public static GameSaveInfo GetRoomSaveInfo(String RoomName, GameSaveInfo defaultIfAbsent) {
        if (! (_roomData.ContainsKey(RoomName)) )
            SetRoomSaveInfo(RoomName, defaultIfAbsent);
        return _roomData[RoomName];
    }

    /*
     * call this function to switch to another room 
     * sceneName: the name of target scene(should be configured in unity build option)
     * _targetPos: the location (Vector3) you wish your player to end up with in the new scene
     */
    public void StartSwitchScene(String sceneName, Vector3 _targetPos) {
        if (switching)
        {
            Settings.DisplayWarning("The attempt to switch to " + sceneName + " was aborted since the scene is already being switched", gameObject);
            return;
        }
        currentProgress = 0;
        switching = true;
        complete = false;
        targetPos = _targetPos;
        MenuObjectClass.EnableMenu();
        Settings.DisableInput();
        // load the future scene
        // it is necessary to wait for the scene to be fully loaded
        // then we can move player, penguin etc. into the new scene, discarding the former. 
        StartCoroutine(
            HandleSceneLoading( sceneName, SceneManager.LoadSceneAsync(sceneName)) );
    }
    /*
     * helper function that is called when the new room is loading; it calls FinishSwitchScene after finishes loading
     * this should not be called from other places!
     */
    private IEnumerator HandleSceneLoading(String sceneName, AsyncOperation sceneLoadOperation) {
        // TODO: play a loading screen
        do {
            UpdateLoadingProgress(sceneLoadOperation);
            yield return new WaitForEndOfFrame();
        } while (!sceneLoadOperation.isDone);
        // upon reaching here, the new scene is loaded (hopefully).
        FinishSwitchScene(sceneName);
    }
    /*
     * this function updates the progress UI according to the load progress
     * should not be called from other places.
     */
    private void UpdateLoadingProgress(AsyncOperation sceneLoadOperation) {
        currentProgress = sceneLoadOperation.progress;
    }
    /*
     * this function is called exactly once when the new scene is fully loaded
     * should not be called from other places.
     */
    private void FinishSwitchScene(String sceneName) {
        // TODO: remove the loading screen
        // teleport player and penguin to new position
        complete = true;
    }
}
