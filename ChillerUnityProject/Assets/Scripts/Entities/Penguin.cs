using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Penguin : Entity
 * 
 * This class is the penguin entity.
 * Such entity should attempt to follow the player.
 *
 */
public class Penguin : Entity {

    // True if the penguin is locked in the cage
    public bool Locked { get; private set; }

    // do not set the follow radius to a small number that the penguin actually pushes the player around the room
    public static float FOLLOW_RADIUS = 0.2f, FOLLOW_SPEED = 0.5f;

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
    }
    // override the AI function: it should try to follow player when far away
    protected override void AI() {
        if (Locked)
        {
            velocity = Vector3.zero;
            return;
        }
        // get the distance and direction to follow
        Player ply = Player.Instance;
        float dist = getCollider().Distance(Player.Instance.getCollider()).distance;
        // follow if not close enough; otherwise, stop movement
        if (dist < FOLLOW_RADIUS) {
            this.velocity = Vector3.zero;
        }
        else {
            Vector3 direction = ply.transform.position - this.transform.position;
            this.velocity = direction.normalized * FOLLOW_SPEED;
        }
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
            gameObject.layer = LayerMask.NameToLayer("Room Physics");
        }
        Locked = isLocked;
    }
}

