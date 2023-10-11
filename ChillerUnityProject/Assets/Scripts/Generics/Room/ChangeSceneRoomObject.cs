using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * An object that changes your current scene upon interaction
 * Commonly known as "doors"
 */
public class ChangeSceneRoomObject : InteractableRoomObject
{
    public String nextSceneName = "";
    public Vector3 targetPos;
    protected override void Interact() {
        if (nextSceneName == "") {
            Debug.Log("ERROR: you did not specify the next scene for the ChangeSceneRoomObject!");
            return;
        }
        GameManager.Instance.startSwitchScene(nextSceneName, targetPos);
    }
}
