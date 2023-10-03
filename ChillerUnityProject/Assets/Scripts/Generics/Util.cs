using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;

/*
 * A helpful static class of methods
 */
public static class Util
{
    /*
     * Returns true if the mouse position overlaps the collider and the keyCode was pressed this frame
     */
    public static bool GetKeyDownWithMouseOverObject(KeyCode keyCode, Collider2D collider)
    {
        return IsMouseOverObject(collider) && Input.GetKeyDown(keyCode);
    }
    /*
     * Returns true if the mouse position overlaps the collider and the keyCode was released this frame
     */
    public static bool GetKeyUpWithMouseOverObject(KeyCode keyCode, Collider2D collider)
    {
        return IsMouseOverObject(collider) && Input.GetKeyUp(keyCode);
    }
    /*
     * Returns true if the mouse position overlaps the collider and the keyCode is pressed
     */
    public static bool GetKeyWithMouseOverObject(KeyCode keyCode, Collider2D collider)
    {
        return IsMouseOverObject(collider) && Input.GetKey(keyCode);
    }
    /*
     * Returns true if the mouse position overlaps the collider
     */
    public static bool IsMouseOverObject(Collider2D collider)
    {
        return collider.OverlapPoint(GetMouseWorldPoint());
    }
    /*
     * Returns the Vector2 position of the mouse in the world
     */
    public static Vector2 GetMouseWorldPoint()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition); ;
    }
}
