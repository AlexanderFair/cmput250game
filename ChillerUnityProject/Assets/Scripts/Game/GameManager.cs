﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Game Manager handles save information and room switching
 */
public class GameManager : MonoBehaviour
{
    public GameObject switchMenuPrefab;

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

    public class GameSaveInfo {
        public static Dictionary<String, object> saveData = new Dictionary<String, object>();
    }

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
        DontDestroyOnLoad(gameObject);
        if (_instanceDefined)
            Debug.Log("Warning: a duplicated game manager instance might be present. ");
        _instance = this;
        _instanceDefined = true;
        _roomData = new Dictionary<String, GameSaveInfo>();

        if(switchMenuPrefab == null)
        {
            throw new NullReferenceException("The switchMenuPrefab is null on the GameManager object");
        }
    }

    // set 
    public static void SetRoomSaveInfo(String RoomName, GameSaveInfo gameInfo) {
        _roomData[RoomName] = gameInfo;
    }
    public static GameSaveInfo GetRoomSaveInfo(String RoomName) {
        return GetRoomSaveInfo(RoomName, new GameSaveInfo());
    }
    public static GameSaveInfo GetRoomSaveInfo(String RoomName, GameSaveInfo defaultIfAbsent) {
        if (! (_roomData.ContainsKey(RoomName)) )
            SetRoomSaveInfo(RoomName, defaultIfAbsent);
        return _roomData[RoomName];
    }

    // call this function to switch to another room
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
        //MenuObjectClass.InstantiateNewMenuElement(switchMenuPrefab);
        MenuObjectClass.EnableMenu();
        Settings.DisableInput();
        // load the future scene
        // it is necessary to wait for the scene to be fully loaded
        // then we can move player, penguin etc. into the new scene, discarding the former. 
        StartCoroutine(
            HandleSceneLoading( sceneName, SceneManager.LoadSceneAsync(sceneName)) );
    }
    // helper function that is called when the new room is loading
    private IEnumerator HandleSceneLoading(String sceneName, AsyncOperation sceneLoadOperation) {
        // TODO: play a loading screen
        do {
            UpdateLoadingProgress(sceneLoadOperation);
            yield return new WaitForEndOfFrame();
        } while (!sceneLoadOperation.isDone);
        // upon reaching here, the new scene is loaded (hopefully).
        FinishSwitchScene(sceneName);
    }
    // this function updates the progress UI according to the load progress
    // TODO: update the progress bar (if we have time to make this feature)
    private void UpdateLoadingProgress(AsyncOperation sceneLoadOperation) {
        currentProgress = sceneLoadOperation.progress;
    }
    // this function is called once the new scene has been fully loaded
    private void FinishSwitchScene(String sceneName) {
        // TODO: remove the loading screen
        // teleport player and penguin to new position
        complete = true;
    }
}
