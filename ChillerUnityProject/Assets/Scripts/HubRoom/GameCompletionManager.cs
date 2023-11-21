using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script that manages the availability of the exit door
public class GameCompletionManager : RoomObjectClass
{
    [Header("Exit door settings")]
    //The door when the player can exit the game
    public GameObject CanExitDoor;
    // the door when the player cannot exit
    public GameObject WallExitDoor;

    // True if all puzzles in the radio room is complete
    // Has to be explicitly set by the user
    public static bool RadioRoomComplete { get; set; } = false;
    // True if all puzzles in the boiler room is complete
    // Has to be explicitly set by the user
    public static bool BoilerRoomComplete {
        get
        {
            return PipeGrid.getPuzzle(PipeGrid.PipePuzzles.HARD).isSolved();
        }
    
    }
    // True if all puzzles in the hub room is complete
    // Has to be explicitly set by the user
    public static bool HubRoomComplete { get; set; } = true;

    //True if all rooms are complete
    public static bool GameComplete { 
        get { 
            return RadioRoomComplete && BoilerRoomComplete && HubRoomComplete;
        } 
    }

    private GameObject door; //the current displayed door
    private bool canPrevExit; //the state of the previous frame

    public override void Start()
    {
        base.Start();
        if (GameComplete)
        {
            Instantiate(CanExitDoor);
            canPrevExit = true;
        }
        else
        {
            Instantiate(WallExitDoor);
            canPrevExit = false;
        }
    }

    protected override void UpdateRoomObject()
    {
        base.UpdateRoomObject();
        if(GameComplete && !canPrevExit)
        {
            Destroy(door);
            Instantiate(CanExitDoor);
        }
        canPrevExit = GameComplete;
    }


}
