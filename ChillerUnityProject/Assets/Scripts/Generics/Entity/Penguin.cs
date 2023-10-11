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
    // do not set the follow radius to a small number that the penguin actually pushes the player around the room
    public static float FOLLOW_RADIUS = 0.2f, FOLLOW_SPEED = 0.5f;

    private static Penguin _instance;
    public static Penguin instance {
        get {
            return _instance;
        }
    }
    public Penguin() {
        if (_instance != null)
            Debug.Log("Warning: a duplicated penguin instance might be present. " + this);
        _instance = this;
    }
    // this should not be destroyed when the scenes switch around.
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // override the AI function: it should try to follow player when far away
    protected override void AI() {
        // get the distance and direction to follow
        Player ply = Player.plyInstance;
        float dist = getCollider().Distance(Player.plyInstance.getCollider()).distance;
        // follow if not close enough; otherwise, stop movement
        if (dist < FOLLOW_RADIUS) {
            this.velocity = Vector3.zero;
        }
        else {
            Vector3 direction = ply.transform.position - this.transform.position;
            this.velocity = direction.normalized * FOLLOW_SPEED;
        }
    }
}

