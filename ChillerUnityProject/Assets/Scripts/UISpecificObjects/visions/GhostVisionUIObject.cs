using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostVisionUIObject : UIObjectClass
{
    public enum RoomGhostState {
        APPEARING, SCARE, IDLE, DESPAWNING
    }

    [Header("Generics")]
    public float despawnChance = 0.4f;
    [Header("Animations")]
    // animation sprite lists
    public Sprite[] appearAnim;
    public Sprite[] scareAnim;
    public Sprite[] idleAnim;
    public Sprite[] despawnAnim;

    // internal variables
    protected RoomGhostState _currState = RoomGhostState.APPEARING;



    // Update is called once per frame
    void Update()
    {
        if(IsUIActive() && !MenuObjectClass.IsMenuActive())
        {
            UpdateUIObject();
            foreach (var anim in spriteAnimators)
            {
                if (anim is GhostVisionUIObjectAnimator) {
                    ((GhostVisionUIObjectAnimator) anim).UpdateGhostAnimation();
                }
                else {
                    anim?.UpdateAnimation();
                }
            }
        }
    }
    /*
     * currently here as a placeholder to prevent possible error due to no override
     */
    protected override void AwakeUIObject() {
        transform.position += new Vector3(Random.Range(-350f, 350f), Random.Range(-175f, 175f), 0f);
        spriteAnimators[0].ChangeAnimation(appearAnim, false);
    }
    /*
     * currently here as a placeholder to prevent possible error due to no override
     */
    protected override void OnDestroyUIObject() {
    }
    /*
     * currently here as a placeholder to prevent possible error due to no override
     */
    protected override void UpdateUIObject() {
    }


    public void changeState() {
        switch (_currState) {
            case RoomGhostState.APPEARING:
                _currState = RoomGhostState.IDLE;
                break;
            case RoomGhostState.SCARE:
                _currState = RoomGhostState.IDLE;
                break;
            case RoomGhostState.IDLE:
                if (Random.Range(0f, 1f) < despawnChance)
                    _currState = RoomGhostState.DESPAWNING;
                // if does not despawn yet
                else if (Random.Range(0f, 1f) < 0.35)
                    _currState = RoomGhostState.SCARE;
                else
                    _currState = RoomGhostState.IDLE;
                break;
            case RoomGhostState.DESPAWNING:
                DestroyImmediate(gameObject);
                return;

        }
        // update sprite animation
        switch (_currState) {
            case RoomGhostState.APPEARING:
                spriteAnimators[0].ChangeAnimation(appearAnim, false);
                break;
            case RoomGhostState.SCARE:
                spriteAnimators[0].ChangeAnimation(scareAnim, false);
                break;
            case RoomGhostState.IDLE:
                spriteAnimators[0].ChangeAnimation(idleAnim, false);
                break;
            case RoomGhostState.DESPAWNING:
                spriteAnimators[0].ChangeAnimation(despawnAnim, false);
                break;
        }
    }
}
