using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PipeGrid
{
    private static Dictionary<String, PipeGrid> ALL_PUZZLES = new Dictionary<String, PipeGrid>();
    private static String currentPuzzle = "PUZZLE";
    private static PuzzleRoomObj triggeredObj;
    public static GameObject currUI;

    public int liquidRemaining, liquidDecrement = 0;
    public Dictionary<(int, int), BasicPipe> PIPE_MAP = new Dictionary<(int, int), BasicPipe>();
    public bool isRotating = false, pendingFlowUpdate = true;
    public PipeGrid(int liquidTotal, String puzzleIndex) {
        liquidRemaining = liquidTotal;
        ALL_PUZZLES[puzzleIndex] = this;
    }
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
    public static void setPuzzle(String puzzleName, PuzzleRoomObj roomObj, GameObject UI) {
        currentPuzzle = puzzleName;
        triggeredObj = roomObj;
        currUI = UI;
        initPuzzle(puzzleName);
    }
    public static PipeGrid getPuzzle() {
        String puzzleIndex = currentPuzzle;
        if (! (ALL_PUZZLES.ContainsKey(puzzleIndex)) ) {
            initPuzzle(puzzleIndex);
        }
        return ALL_PUZZLES[puzzleIndex];
    }

    public static class Directions {
        // index: the index in which the connection state is saved in BasicPipe
        public static int RIGHT = 0, DOWN = 1, LEFT = 2, UP = 3, TOTAL_DIRECTIONS = 4;
        public static int getOpposite(int direction) {
            return (direction + (TOTAL_DIRECTIONS / 2)) % TOTAL_DIRECTIONS;
        }
    }
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
