using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the room object
public class HubJigsawRoomObject : JigsawRoomObject
{
    public static bool Complete { get; private set; } = false;

    public override void SetSolved()
    {
        Complete = true;
    }

    public override bool IsSolved()
    {
        return Complete;
    }
}
