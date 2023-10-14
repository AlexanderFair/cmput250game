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
    private static Player _plyInstance = null;
    private static bool _instanceDefined = false;
    public static Player plyInstance {
        get {
            if (!_instanceDefined)
                Debug.Log("Warning: no valid player instance is present. ");
            return _plyInstance;
        }
    }
    // this is the acceleration/fixed speed per second. It may need additional tweaks to yield a satisfying result.
    // which one to use is determined by MOVE_WITH_ACCELERATION variable.
    public float MOVE_ACCELERATION_PER_SECOND = 2.5f, MOVE_SPEED = 0.75f;
    // is the player supposed to move with accelration or move with a fixed speed?
    public bool MOVE_WITH_ACCELERATION = false;

    // this should not be destroyed when the scenes switch around.
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (_instanceDefined)
            Debug.Log("Warning: a duplicated player instance might be present. ");
        _plyInstance = this;
        _instanceDefined = true;
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
        Vector3 moveDir = new Vector3(horMoveDir, verMoveDir, 0);
        if (MOVE_WITH_ACCELERATION) {
            moveDir *= Time.deltaTime * MOVE_ACCELERATION_PER_SECOND;
            accelerate(moveDir, -1);
        }
        else {
            moveDir *= MOVE_SPEED;
            this.velocity = moveDir;
        }
    }
}
