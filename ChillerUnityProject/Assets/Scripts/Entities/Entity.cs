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


    /* Called when the object is updated and the UI and Menu is not active */
    protected void FixedUpdate() {
        AI();
        movement();
    }

    /*
     * get the rigidbody2D attatched
     */
    public Rigidbody2D getRigidBody() {
        return GetComponent<Rigidbody2D>();
    }
    /*
     * get the collider2D attatched
     */
    public Collider2D getCollider() {
        return GetComponent<Collider2D>();
    }
    /*
     * get the velocity
     * note that the velocity here is in units/sec, it HAS NOTHING TO DO WITH Time.deltaTime
     */
    public Vector3 getVelocity() {
        return velocity;
    }
    /*
     * set the velocity
     * note that the velocity here is in units/sec, it HAS NOTHING TO DO WITH Time.deltaTime
     */
    public void setVelocity(Vector3 newVel) {
        velocity = newVel;
    }
    /*
     * accelerate the entity, max speed can be set to negative to be ignored
     * if it is not negative, the speed after acceleration would be scaled down to the max speed
     * NOTE: if this is called on a per-update basis, you may need to consider Time.deltaTime
     */
    public void accelerate(Vector3 acceleration, float maxSpeed) {
        velocity += acceleration;
        if (maxSpeed > 0) {
            double velLenSqr = velocity.sqrMagnitude;
            if (velLenSqr > maxSpeed * maxSpeed) {
                velocity *= maxSpeed / velocity.magnitude;
            }
        }
    }


    /*
     * player movement, penguin follow etc.
     */
    protected abstract void AI();
    /*
     * movement tick
     * AI stuff do not belong here. This is purely movement tick.
     */
    protected void movement() {
        // move

        getRigidBody().MovePosition(getRigidBody().position + (Vector2)velocity * Time.fixedDeltaTime);
        //getRigidBody().velocity = velocity;

        // speed decay
        // a^x = speed multiplier per sec, where x = amount slows triggered within 1 sec = 1/deltatime, a = actual speed multi
        // a = speed multi per sec ^ 1/x = speed multi per sec ^ deltatime
        velocity *= Mathf.Pow(speedDecayMultiplierPerSecond, Time.fixedDeltaTime); 
    }
}
