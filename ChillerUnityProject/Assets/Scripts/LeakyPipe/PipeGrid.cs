using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 *
 * This is the class that "caches" ALL puzzle information.
 *
 * It also has helper functions to validate a pipe puzzle, 
 * definitions for pipe rotations 
 * as well as a class for pipe connection tracing.
 *
 */
public class PipeGrid {
    // the enumeration of all levels
    public enum PipePuzzles {
        INTRO, MEDIUM, HARD
    }
    // pseudo "enum" class for direction declarition
    public static class Directions {
        // index: the index in which the connection state is saved in BasicPipe
        public static int RIGHT = 0, DOWN = 1, LEFT = 2, UP = 3, TOTAL_DIRECTIONS = 4;
        public static int getOpposite(int direction) {
            return (direction + (TOTAL_DIRECTIONS / 2)) % TOTAL_DIRECTIONS;
        }
    }
    // the class useful when tracing the pipe puzzle
    public class PipeTraceResult {
        public HashSet<BasicPipe> allPipes;
        private HashSet<BasicPipe> visited;
        // constructor
        public PipeTraceResult(BasicPipe startPipe) {
            startTracePipe(startPipe);
        }
        // clears info and traces the connected pipes from a starting point
        public void startTracePipe(BasicPipe startPipe) {
            allPipes = new HashSet<BasicPipe>();
            visited = new HashSet<BasicPipe>();
            // trace
            traceSinglePipe(startPipe);
        }
        // recursively determine all connected components
        public void traceSinglePipe(BasicPipe startPipe) {
            if (visited.Contains(startPipe))
                return;
            allPipes.Add(startPipe);
            visited.Add(startPipe);
            // iterate through nearby pipes
            ArrayList queryResult = startPipe.getConnectedPipes();
            foreach (BasicPipe connectedPipe in queryResult) {
                traceSinglePipe(connectedPipe);
            }
        }
    }

    // fields that keep track of puzzle information
    private static Dictionary<PipePuzzles, PipeGrid> ALL_PUZZLES = new Dictionary<PipePuzzles, PipeGrid>();
    // those items below should keep track of "current" puzzle
    private static PipePuzzles currentPuzzle = PipePuzzles.INTRO;
    private static PuzzleRoomObj _triggeredObj;
    public static PuzzleRoomObj triggeredRoomObj {
        get {
            return _triggeredObj;
        }
    }
    public static GameObject currUI;
    // this is the "reference", or "origin" of the entire puzzle
    BasicPipe originRef = null;
    // these non-static fields are responsible for saving the current puzzle properties
    public int liquidRemaining, liquidDecrement = 0;
    public Dictionary<(int, int), BasicPipe> PIPE_MAP = new Dictionary<(int, int), BasicPipe>();
    public bool isRotating = false, pendingFlowUpdate = true;
    // constructor, it takes in a number representing the total liquid and internal puzzle name, or index.
    public PipeGrid(int liquidTotal, PipePuzzles puzzleIndex) {
        liquidRemaining = liquidTotal;
        ALL_PUZZLES[puzzleIndex] = this;
    }
    // note that different puzzles have their initial liquid amount hard-coded in here.
    // this should be called when you reset a puzzle; if you wish to INITIALIZE a puzzle, use function setPuzzle.
    public static void initPuzzle(PipePuzzles puzzleIndex, int puzzleLiquidTotal) {
        new PipeGrid(puzzleLiquidTotal, puzzleIndex);
    }
    // this sets the current puzzle and additional information.
    public static void setPuzzle(PipePuzzles puzzleName, PuzzleRoomObj roomObj, GameObject UI) {
        currentPuzzle = puzzleName;
        _triggeredObj = roomObj;
        currUI = UI;
        initPuzzle(puzzleName, roomObj.liquidAmount);
    }
    // gets the current puzzle
    public static PipeGrid getPuzzle() {
        PipePuzzles puzzleIndex = currentPuzzle;
        if (! (ALL_PUZZLES.ContainsKey(puzzleIndex)) ) {
            initPuzzle(puzzleIndex, 0);
        }
        return ALL_PUZZLES[puzzleIndex];
    }
    // calculate and save rotation & the coordinate position of a pipe, registering it to the current pipe grid
    public void registerPipe(BasicPipe pipe) {
        // set origin reference to the first pipe registered
        if (originRef == null)
            originRef = pipe;
        // get the sprite renderer of pipe
        SpriteRenderer attatchedSpriteRenderer = pipe.getAttatchedRenderer();
        SpriteRenderer originRefSpriteRenderer = originRef.getAttatchedRenderer();
        // generate grid x and y based on coords
        float w = attatchedSpriteRenderer.bounds.size.x, h = attatchedSpriteRenderer.bounds.size.y;
        float currObjX = attatchedSpriteRenderer.bounds.center.x, currObjY = attatchedSpriteRenderer.bounds.center.y;
        float refObjX = originRefSpriteRenderer.bounds.center.x, refObjY = originRefSpriteRenderer.bounds.center.y;
        pipe.gridX = (int) Mathf.Round( (currObjX - refObjX) / w );
        pipe.gridY = (int) Mathf.Round( (currObjY - refObjY) / h );
        (int, int) coord = (pipe.gridX, pipe.gridY);
        // regularize the location of pipe component with coord calculated
        pipe.transform.position = new Vector3(refObjX + w * pipe.gridX, refObjY + h * pipe.gridY, originRef.transform.position.z);
        // load current pipe into pipe grid
        // logs an error message when two pipes are at the same position
        if (PIPE_MAP.ContainsKey(coord)) {
            Debug.Log("Two pipes are at the same position? Coord: " + coord + "||" + pipe.transform.position);
            GameObject.Destroy(pipe);
        }
        else 
            PIPE_MAP.Add(coord, pipe);
        // generate rotation based on component rotation
        double compRot = pipe.transform.rotation.eulerAngles.z;
        // switch it to positive to prevent unexpected behaviour of modulo operation on negative numbers
        if (compRot < 0)
            compRot += 90 * Math.Ceiling(compRot / -90);
        pipe.rotation = ((int) Math.Round(4f - (compRot / 90f) )) % 4;
    }

    // is this puzzle solved? default that no leak is allowed; you can specify it though.
    public bool isSolved() {
        return isSolved(true);
    }
    public bool isSolved(bool noLeak) {
        // if no pipe is present (initialized as a placeholder?), return false
        if (PIPE_MAP.Count == 0)
            return false;
        // if water is gone
        if (liquidRemaining <= 0)
            return false;
        // if the pipe has leak and leak is not allowed
        if (noLeak && liquidDecrement > 0)
            return false;
        // if any exit is not yet connected
        foreach( KeyValuePair<(int, int), BasicPipe> entry in PIPE_MAP ) {
            BasicPipe pipe = entry.Value;
            if (pipe is PipeOutput && ! (pipe.isConnected))
                return false;
        }
        // yay!
        GameCompletionManager.BoilerRoomComplete = true;//CHANGE
        Debug.Log("Complete B");
        return true;
    }

    
}
