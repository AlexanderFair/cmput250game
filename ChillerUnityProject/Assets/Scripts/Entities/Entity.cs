using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Entity : RoomObjectClass
 * 
 * This class implements the basic structure of Entity.
 * More specific child classes are expected to be directly used in game scenes.
 * 
 * Subclasses must implement the UpdateRoomObject. 
 *
 */
public abstract class Entity : RoomObjectClass
{
    protected Vector3 velocity = Vector3.zero;
    public float speedDecayMultiplierPerSecond = 0.02f;

    private bool aiUpdatedSinceLastMovementUpdate = false;

    /* Called when the object is updated and the UI and Menu is not active */
    protected override void UpdateRoomObject() {
        AI();
        aiUpdatedSinceLastMovementUpdate = true;
    }

    public Rigidbody2D getRigidBody() {
        return GetComponent<Rigidbody2D>();
    }
    public Collider2D getCollider() {
        return GetComponent<Collider2D>();
    }
    public Vector3 getVelocity() {
        return velocity;
    }
    public void setVelocity(Vector3 newVel) {
        velocity = newVel;
    }
    // accelerate the entity, max speed can be set to negative to be ignored
    // if it is not negative, the speed after acceleration would be scaled down to the max speed
    public void accelerate(Vector3 acceleration, float maxSpeed) {
        velocity += acceleration;
        if (maxSpeed > 0) {
            double velLenSqr = velocity.sqrMagnitude;
            if (velLenSqr > maxSpeed * maxSpeed) {
                velocity *= maxSpeed / velocity.magnitude;
            }
        }
    }

    public void FixedUpdate()
    {
        if (aiUpdatedSinceLastMovementUpdate)
        {
            aiUpdatedSinceLastMovementUpdate = false;
            movement();
        }
    }

    // player movement, penguin follow etc.
    protected abstract void AI();
    // movement
    protected void movement() {
        // move

        getRigidBody().MovePosition(getRigidBody().position + (Vector2)velocity * Time.deltaTime);
        //getRigidBody().velocity = velocity;

        // speed decay
        // a^x = speed multiplier per sec, where x = amount slows triggered within 1 sec = 1/deltatime, a = actual speed multi
        // a = speed multi per sec ^ 1/x = speed multi per sec ^ deltatime
        velocity *= Mathf.Pow(speedDecayMultiplierPerSecond, Time.deltaTime); 
    }
}
