using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//black screen to go behind main menu
public class CutSceneBlackBackground : RoomObjectClass
{
    public static bool Complete;
    public static CutSceneBlackBackground Instance { get; private set; }
    private static bool defined = false;

    private bool thisdefine = false;

    public override void Start()
    {
        base.Start();
        if(defined)
        {
            DestroyImmediate(Instance);
        }
        thisdefine = true;
        defined = true;
        Instance = this;
        if (Complete)
        {
            DisableBackground();
        }
    }

    public void DisableBackground()
    {
        Complete = true;
        DestroyImmediate(gameObject);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        if (thisdefine)
        {
            defined = false;
            Instance = null;
        }
    }
}
