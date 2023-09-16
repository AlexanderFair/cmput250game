using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSquare : PipeSquare
{

    public DoorSquare(int x, int y, int pipeType, int rotation, Sprite emSprite, Sprite watSprite, string name) : base(x, y, pipeType, rotation, emSprite, watSprite, name)
    {
    }

    public override void SetWater(bool has)
    {
        base.SetWater(has);
        Debug.Log("Found " + Name);
    }
}
