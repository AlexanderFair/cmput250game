using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLifetimeObject : RoomObjectClass
{
    public float lifetime = 0;

    private float life = 0;

    protected override void UpdateRoomObject()
    {
        base.UpdateRoomObject();
        life += Time.deltaTime;
        if(life >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
