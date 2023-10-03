using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Settings for the game
 */
public static class Settings
{
    public static class Controls
    {
        public static KeyCode Interact = KeyCode.F;
        public static KeyCode LeftClick = KeyCode.Mouse0;
    }

    public static class Player
    {
        /*
         * The max distance required between the collider of the object and the collider of the player 
         * before the object becomes available for interaction in the room 
         */
        public static float InteractDistance = 1f;
    }
}
