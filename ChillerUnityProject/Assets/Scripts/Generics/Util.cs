using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;

/*
 * A helpful static class of methods
 */
public static class Util
{
    public static System.Random randomInstance = new System.Random();
    /*
     * Returns true if the mouse position overlaps the collider and the keyCode was pressed this frame
     * 
     * If forceInputEnabled, the keyPress is returned independent of if Input is enabled in settings
     */
    public static bool GetKeyDownWithMouseOverObject(Settings.Controls control, Collider2D collider, bool forceInputEnabled = false)
    {
        return IsMouseOverObject(collider) && control.GetKeyDown(forceInputEnabled);
    }
    /*
     * Returns true if the mouse position overlaps the collider and the keyCode was released this frame
     * 
     * If forceInputEnabled, the keyPress is returned independent of if Input is enabled in settings
     */
    public static bool GetKeyUpWithMouseOverObject(Settings.Controls control, Collider2D collider, bool forceInputEnabled = false)
        {
            return IsMouseOverObject(collider) && control.GetKeyUp(forceInputEnabled);
        }
    /*
     * Returns true if the mouse position overlaps the collider and the keyCode is pressed
     * 
     * If forceInputEnabled, the keyPress is returned independent of if Input is enabled in settings
     */
    public static bool GetKeyWithMouseOverObject(Settings.Controls control, Collider2D collider, bool forceInputEnabled = false)
    {
        return IsMouseOverObject(collider) && control.GetKey(forceInputEnabled);
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

    // Retunrs a radnom item from the array
    public static T ChooseRandom<T>(T[] vals)
    {
        if(vals.Length == 0)
        {
            return default;
        }
        return vals[randomInstance.Next(vals.Length)];
    }
    
    // Retunrs a radnom item from the list
    public static T ChooseRandom<T>(IEnumerable<T> vals)
    {
        if(vals.Count() == 0)
        {
            return default;
        }
        return vals.ElementAt(randomInstance.Next(vals.Count()));
    }

}
