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
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += (Vector3)Vector2.right * Time.deltaTime * 5f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += (Vector3)Vector2.left * Time.deltaTime * 5f;
        }
    }
}
