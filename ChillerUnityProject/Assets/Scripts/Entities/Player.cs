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
    public static Player Instance {
        get {
            if (!_instanceDefined)
                Debug.Log("Warning: no valid player instance is present. ");
            return _plyInstance;
        }
    }
    // this is the acceleration per second. It may need additional tweaks to yield a satisfying result.
    public float MOVE_ACCELERATION_PER_SECOND = 2.5f;

    // animation sprite lists
    public Sprite[] idleAnim;
    public Sprite[] walkEastAnim;
    public Sprite[] walkWestAnim;
    public Sprite[] walkSouthAnim;
    public Sprite[] walkNorthAnim;

    // this should not be destroyed when the scenes switch around.
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (_instanceDefined)
        {
            DestroyImmediate(gameObject);
            return;
        }
        _plyInstance = this;
        _instanceDefined = true;
    }
    
    protected override void AI() {
        // positive: right & up
        float horMoveDir = 0, verMoveDir = 0;

        // sprite animation changes depending on which key is pressed
        if (Settings.Controls.MoveUp.GetKey()) {
            spriteAnimations[0].ChangeAnimation(walkNorthAnim, false);
            verMoveDir ++;
        } else if (Settings.Controls.MoveDown.GetKey()) {
            spriteAnimations[0].ChangeAnimation(walkSouthAnim, false);
            verMoveDir --;
        } else if (Settings.Controls.MoveLeft.GetKey()) {
            spriteAnimations[0].ChangeAnimation(walkWestAnim, false);
            horMoveDir --;
        } else if (Settings.Controls.MoveRight.GetKey()) {
            spriteAnimations[0].ChangeAnimation(walkEastAnim, false);
            horMoveDir ++;
        } else {
            // idle animation
            spriteAnimations[0].ChangeAnimation(idleAnim);
        }
        // accelerate
        Vector3 accel = new Vector3(horMoveDir * MOVE_ACCELERATION_PER_SECOND, verMoveDir * MOVE_ACCELERATION_PER_SECOND, 0);
        accel *= Time.deltaTime;
        accelerate(accel, -1);
    }
}