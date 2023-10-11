using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Player : Entity
 * 
 * This class is the player entity.
 * Such entity should move when WASD is pressed. Furthermore, there should be only one player in the entire game.
 *
 */
public class Player : Entity {
    private static Player _plyInstance;
    public static Player plyInstance {
        get {
            return _plyInstance;
        }
    }
    // this is the acceleration per second. It may need additional tweaks to yield a satisfying result.
    public float MOVE_ACCELERATION_PER_SECOND = 2.5f;
    
    public Player() {
        if (_plyInstance != null)
            Debug.Log("Warning: a duplicated player instance might be present. " + this);
        _plyInstance = this;
    }

    // this should not be destroyed when the scenes switch around.
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    protected override void AI() {
        // positive: right & up
        float horMoveDir = 0, verMoveDir = 0;
        if (Input.GetKey(KeyCode.W))
            verMoveDir ++;
        if (Input.GetKey(KeyCode.S))
            verMoveDir --;
        if (Input.GetKey(KeyCode.A))
            horMoveDir --;
        if (Input.GetKey(KeyCode.D))
            horMoveDir ++;
        // accelerate
        Vector3 accel = new Vector3(horMoveDir * MOVE_ACCELERATION_PER_SECOND, verMoveDir * MOVE_ACCELERATION_PER_SECOND, 0);
        accel *= Time.deltaTime;
        accelerate(accel, -1);
    }
}
