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
    public bool useAccelerationMode = true;
    public static Player plyInstance {
        get {
            if (!_instanceDefined)
                Debug.Log("Warning: no valid player instance is present. ");
            return _plyInstance;
        }
    }
    // this is the acceleration per second. It may need additional tweaks to yield a satisfying result.
    public float MOVE_ACCELERATION_PER_SECOND = 2.5f, FIXED_MOVE_SPEED_PER_SEC = 0.75f;

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
        // movement
        Vector3 moveDir = new Vector3(horMoveDir, verMoveDir, 0);
        // accelerate
        if (useAccelerationMode) {
            moveDir *= MOVE_ACCELERATION_PER_SECOND * Time.deltaTime;
            accelerate(moveDir, -1);
        }
        // set fixed velocity
        else {
            moveDir *= FIXED_MOVE_SPEED_PER_SEC;
            velocity = moveDir;
        }
    }
}
