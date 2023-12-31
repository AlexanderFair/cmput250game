﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/*
 * Penguin : Entity
 * 
 * This class is the penguin entity.
 * Such entity should attempt to follow the player.
 *
 */
public class Penguin : Entity {

    [Header("Penguin Add")]
    public Sprite[] rightAnim;
    public Sprite[] rightIdleAnim;
    public Sprite[] leftAnim;
    public Sprite[] leftIdleAnim;
    public AnimationSpriteClass animator;
    // True if the penguin is locked in the cage
    public bool Locked { get; private set; }

    // do not set the follow radius to a small number that the penguin actually pushes the player around the room
    public float FOLLOW_RADIUS = 0.2f, FOLLOW_SPEED = 0.5f;

    private bool wasLocked = false;
    private float lockTime = 0;
    public float lockMovementTime = 1f;
    public Vector2 unlockVelocity;
    public float sortOrderTimer = 0.5f;
    private float sortTime = 0f;
    public SpriteRenderer cageForegroundSprite;

    private static Penguin _instance;
    private static bool _instanceDefined = false;
    public static Penguin instance {
        get {
            if (!_instanceDefined)
                Debug.Log("Warning: no valid penguin instance is present. ");
            return _instance;
        }
    }
    
    // this should not be destroyed when the scenes switch around.
    public void Awake()
    {
        //if (_instanceDefined)
            //Debug.Log("Warning: a duplicated penguin instance might be present. ");
        _instance = this;
        _instanceDefined = true;
        wasLocked = Locked;
    }

    public override void Start()
    {
        base.Start();
        animator.ChangeAnimation(rightIdleAnim);
    }

    private bool moving = false;
    private bool lookLeft = false;
    // override the AI function: it should try to follow player when far away
    protected override void AI() {
        bool calcVel = false;
        if (Locked)
        {
            velocity = Vector3.zero;
            wasLocked = true;
            return;
        }
        if (wasLocked)
        {
            if(lockTime < lockMovementTime)
            {
                lockTime += Time.deltaTime;
                velocity = unlockVelocity;
                calcVel = true;

                if(sortTime +Time.deltaTime >= sortOrderTimer && sortTime < sortOrderTimer) {
                    cageForegroundSprite.sortingOrder = 0;
                }

                sortTime += Time.deltaTime;
            }
            else
            {
                wasLocked = false;
                gameObject.layer = LayerMask.NameToLayer("Room Physics");
                GetComponent<SpriteRenderer>().sortingOrder = 0;
            }
        }
        if (!calcVel)
        {
            // get the distance and direction to follow
            Player ply = Player.Instance;
            float dist = getCollider().Distance(Player.Instance.getCollider()).distance;
            // follow if not close enough; otherwise, stop movement
            if (dist < FOLLOW_RADIUS)
            {
                this.velocity = Vector3.zero;
            }
            else
            {
                Vector3 direction = ply.transform.position - this.transform.position;
                this.velocity = direction.normalized * FOLLOW_SPEED;
            }
        }
        

        bool switchAnim = false;

        if (velocity.x < 0)
        {
            if (!lookLeft) switchAnim = true;
            lookLeft = true;
        }
        else if(velocity.x > 0)
        {
            if (lookLeft) switchAnim = true;
            lookLeft = false;
        }

        if (velocity.sqrMagnitude > 0)
        {
            if(!moving)
            {
                switchAnim = true;
            }
            moving = true;
        }
        else
        {
            if (moving)
            {
                switchAnim = true;
            }
            moving = false;
        }

        if (switchAnim)
        {
            SwitchAnimation();
        }
    }

    private void SwitchAnimation()
    {
        animator.ChangeAnimation(lookLeft ? (moving ? leftAnim : leftIdleAnim) : (moving ? rightAnim : rightIdleAnim));
    }

    /*
     * If set to true, the penguins physics are disabled and the penguin will no longer move
     * If set to false, the penguins physics are reenabled and the penguin will move
     */
    public void SetLockedInCage(bool isLocked)
    {
        if (isLocked)
        {
            gameObject.layer = LayerMask.NameToLayer("Room");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Room Physics - Penguin");
            cageForegroundSprite.sortingOrder = -1;
        }
        Locked = isLocked;
    }
}

