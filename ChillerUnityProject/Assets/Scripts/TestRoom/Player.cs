using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : RoomObjectClass
{
    public Collider2D interactCollider;
    private static Player _instance;
    public static Player Instance { get { return _instance; } }

    public void Awake()
    {
        _instance = this;
    }
    

    protected override void UpdateRoomObject()
    {
        
    }
}
