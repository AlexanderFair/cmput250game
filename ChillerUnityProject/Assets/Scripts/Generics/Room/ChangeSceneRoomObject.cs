using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * An object that changes your current scene upon interaction
 * Commonly known as "doors"
 */
public class ChangeSceneRoomObject : CollisionInteractableRoomObject
{
    [Header("Change Scene Settings")]
    public String nextSceneName = "";
    public Vector3 playerStartPosition;

    public override void Start()
    {
        base.Start();
        if(nextSceneName == "")
        {
            Settings.DisplayError("A scene name was not provided for the scene changer", gameObject);
        }
        if(playerStartPosition == Vector3.zero) 
        {
            Settings.DisplayWarning("The selected startposition is zero", gameObject);
        }
    }

    protected override void Collision(Collider2D _) {
        Settings.DisplayWarning("Changing scenes to " + nextSceneName + ". Triggered by "+_.name, gameObject);
        if (nextSceneName == "") {
            Settings.DisplayError("You did not specify the next scene for the ChangeSceneRoomObject!", gameObject);
            return;
        }
        
        GameManager.Instance.StartSwitchScene(nextSceneName, playerStartPosition);
    }
}
