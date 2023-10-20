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
public class PipeGrid
{
    // fields that keep track of puzzle information
    private static Dictionary<String, PipeGrid> ALL_PUZZLES = new Dictionary<String, PipeGrid>();
    // those items below should keep track of "current" puzzle
    private static String currentPuzzle = "PUZZLE";
    private static PuzzleRoomObj _triggeredObj;
    public static PuzzleRoomObj triggeredRoomObj {
        get {
            return _triggeredObj;
        }
    }
    public static GameObject currUI;
    // these non-static fields are responsible for saving the current puzzle properties
    public int liquidRemaining, liquidDecrement = 0;
    public Dictionary<(int, int), BasicPipe> PIPE_MAP = new Dictionary<(int, int), BasicPipe>();
    public bool isRotating = false, pendingFlowUpdate = true;
    // constructor, it takes in a number representing the total liquid and internal puzzle name, or index.
    public PipeGrid(int liquidTotal, String puzzleIndex) {
        liquidRemaining = liquidTotal;
        ALL_PUZZLES[puzzleIndex] = this;
    }
    // note that different puzzles have their initial liquid amount hard-coded in here.
    // this should be called when you reset a puzzle; if you wish to INITIALIZE a puzzle, use function setPuzzle.
    public static void initPuzzle(String puzzleIndex) {
        int puzzleLiquidTotal;
        switch (puzzleIndex) {
            // tutorial
            case "tutorial":
                puzzleLiquidTotal = 50;
                break;
            default:
                puzzleLiquidTotal = 100;
                break;
        }
        new PipeGrid(puzzleLiquidTotal, puzzleIndex);
    }
    // this sets the current puzzle and additional information.
    public static void setPuzzle(String puzzleName, PuzzleRoomObj roomObj, GameObject UI) {
        currentPuzzle = puzzleName;
        _triggeredObj = roomObj;
        currUI = UI;
        initPuzzle(puzzleName);
    }
    // gets the current puzzle
    public static PipeGrid getPuzzle() {
        String puzzleIndex = currentPuzzle;
        if (! (ALL_PUZZLES.ContainsKey(puzzleIndex)) ) {
            initPuzzle(puzzleIndex);
        }
        return ALL_PUZZLES[puzzleIndex];
    }

    // is this puzzle solved? default that no leak is allowed; you can specify it though.
    public bool isSolved() {
        return isSolved(true);
    }
    public bool isSolved(bool noLeak) {
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
        return true;
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
}
