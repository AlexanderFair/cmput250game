using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Player : Entity
 * 
 * This class is the player entity.
 * Such entity should move when WASD is pressed. 
 * Furthermore, there should be only one player in the entire game.
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
    [Header("Movement")]
    // this is the acceleration per second. It may need additional tweaks to yield a satisfying result.
    // DEPRECATED: currently a new movement system is used
    public float MOVE_ACCELERATION_PER_SECOND = 2.5f;
    // the movement is continuous: a movement direction is specified, then the player moves
    // for MOVE_DURATION_SEC amont of time, move according to the moving direction with speed MOVE_SPEED.
    // this system is mainly implemented to prevent funny sprite twitch when the player quickly alternates between A and D 
    public float MOVE_DURATION_SEC = 0.1f, MOVE_SPEED = 250;
    public float movement_progress = 0f;
    public float speedMultiplier = 1f;
    public Vector3 moveDir = Vector3.zero;
    [Header("Animations")]
    // animation sprite lists
    public Sprite[] idleAnim;
    public Sprite[] walkEastAnim;
    public Sprite[] walkWestAnim;
    public Sprite[] walkSouthAnim;
    public Sprite[] walkNorthAnim;

    [Header("Sound Effects")]
    public AudioClip[] walkingEffects;
    // at the start of which frame(s) should a step sound be played?
    public int[] walkSoundFrames;
    // internal variable
    private bool lastWalkSoundPlayed = true;

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
        // should not have speed decay.
        speedDecayMultiplierPerSecond = 1;
    }
    
    protected override void AI() {
        // move speed should align with sprite update speed
        // so the player does not appear to be "drifting" on ground
        spriteAnimators[0].repetitionFactor = 1 / speedMultiplier;
        // update velocity (otherwise, the player seems to slide after movement ends)
        this.velocity = moveDir;
        // update movement direction if idle
        if (movement_progress == 0f) {
            // positive: right & up
            float horMoveDir = 0, verMoveDir = 0;

            // sprite animation changes depending on which key is pressed
            if (Settings.Controls.MoveUp.GetKey()) {
                verMoveDir ++;
            }
            if (Settings.Controls.MoveDown.GetKey()) {
                verMoveDir --;
            }
            if (Settings.Controls.MoveLeft.GetKey()) {
                horMoveDir --;
            }
            if (Settings.Controls.MoveRight.GetKey()) {
                horMoveDir ++;
            }
            
            moveDir = new Vector3(horMoveDir, verMoveDir, 0);
            moveDir.Normalize();
            moveDir *= MOVE_SPEED * speedMultiplier;
            // start movement if velocity is non-zero
            if (moveDir.sqrMagnitude > 1e-5)
                movement_progress = 1e-9f;
        }
        // update sprite animation
        {
            // north
            if (moveDir.y > 1e-5)
                spriteAnimators[0].ChangeAnimation(walkNorthAnim, false);
            // south
            else if (moveDir.y < -1e-5)
                spriteAnimators[0].ChangeAnimation(walkSouthAnim, false);
            // east
            else if (moveDir.x > 1e-5)
                spriteAnimators[0].ChangeAnimation(walkEastAnim, false);
            // west 
            else if (moveDir.x < -1e-5)
                spriteAnimators[0].ChangeAnimation(walkWestAnim, false);
            // idle
            else
                spriteAnimators[0].ChangeAnimation(idleAnim);
        }
        // step sound
        // this section should be placed right here!
        // otherwise, other movement check will generate bugs
        if( moveDir.sqrMagnitude > 1e-5 )
        {
            //Debug.Log("animIdx(" + spriteAnimators[0].CurrentFrame + "), dt(" + Time.deltaTime + "), FPS(" + Settings.FloatValues.FPS.Get() + ")");
            bool currAnimHasSound = false;
            foreach (int soundFrame in walkSoundFrames) {
                if (soundFrame == spriteAnimators[0].CurrentFrame) {
                    currAnimHasSound = true;
                    break;
                }
            }
            if (currAnimHasSound && !lastWalkSoundPlayed)
            {
                AudioHandler.Instance.playSoundEffect(Util.ChooseRandom(walkingEffects));
            }
            lastWalkSoundPlayed = currAnimHasSound;
        }
        // if not moving, the next move step must also have a sound effect
        else {
            lastWalkSoundPlayed = false;
        }
        // move until timeout, then wait for next action
        if (movement_progress > 0f) {
            movement_progress += Time.fixedDeltaTime;
            // timeout
            if (movement_progress >= MOVE_DURATION_SEC) {
                movement_progress = 0f;
                moveDir = Vector3.zero;
            }
        }
    }
}
