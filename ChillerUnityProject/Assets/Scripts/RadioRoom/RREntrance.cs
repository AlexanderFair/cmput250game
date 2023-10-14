using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RREntrance : ChangeSceneRoomObject
{
    [Header("Entrance Settings")]
    public string prompt;
    public override void Start()
    {
        base.Start();
        DialogDisplay.NewDialog(prompt, AnimationSpriteClass.NULL_STRUCT);

    }
}

//The weather can become frigid cold in the evenings, even for the magestic penguin.