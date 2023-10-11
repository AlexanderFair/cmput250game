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
    private static Dictionary<String, GameSaveInfo> _roomData;
    private static GameManager _instance;
    private static bool _instanceDefined = false;
    public static GameManager Instance {
        get {
            if (!_instanceDefined)
                Debug.Log("Warning: no valid game manager instance is present. ");
            return _instance;
        }
    }

    public class GameSaveInfo {
        public static Dictionary<String, object> saveData = new Dictionary<String, object>();
    }

    // this should not be destroyed when the scenes switch around.
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (_instanceDefined)
            Debug.Log("Warning: a duplicated game manager instance might be present. ");
        _instance = this;
        _instanceDefined = true;
        _roomData = new Dictionary<String, GameSaveInfo>();
    }

    // set 
    public static void setRoomSaveInfo(String RoomName, GameSaveInfo gameInfo) {
        _roomData[RoomName] = gameInfo;
    }
    public static GameSaveInfo getRoomSaveInfo(String RoomName) {
        return getRoomSaveInfo(RoomName, new GameSaveInfo());
    }
    public static GameSaveInfo getRoomSaveInfo(String RoomName, GameSaveInfo defaultIfAbsent) {
        if (! (_roomData.ContainsKey(RoomName)) )
            setRoomSaveInfo(RoomName, defaultIfAbsent);
        return _roomData[RoomName];
    }

    // call this function to switch to another room
    public void startSwitchScene(String sceneName, Vector3 targetPos) {
        // load the future scene
        // it is necessary to wait for the scene to be fully loaded
        // then we can move player, penguin etc. into the new scene, discarding the former. 
        StartCoroutine(
            handleSceneLoading( sceneName, SceneManager.LoadSceneAsync(sceneName), targetPos ) );
    }
    // helper function that is called when the new room is loading
    private IEnumerator handleSceneLoading(String sceneName, AsyncOperation sceneLoadOperation, Vector3 targetPos) {
        // TODO: play a loading screen
        do {
            updateLoadingProgress(sceneLoadOperation);
            yield return new WaitForEndOfFrame();
        } while (!sceneLoadOperation.isDone);
        // upon reaching here, the new scene is loaded (hopefully).
        finishSwitchScene(sceneName, targetPos);
    }
    // this function updates the progress UI according to the load progress
    // TODO: update the progress bar (if we have time to make this feature)
    private void updateLoadingProgress(AsyncOperation sceneLoadOperation) {
        float progress = sceneLoadOperation.progress;
    }
    // this function is called once the new scene has been fully loaded
    private void finishSwitchScene(String sceneName, Vector3 targetPos) {
        // TODO: remove the loading screen
        // teleport player and penguin to new position
        Player.plyInstance.transform.position = targetPos;
        Penguin.instance.transform.position = targetPos;
    }
}
