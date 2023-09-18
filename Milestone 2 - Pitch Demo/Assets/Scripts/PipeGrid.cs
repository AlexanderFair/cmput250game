using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PipeGrid
{
    public static Dictionary<(int, int), BasicPipe> PIPE_MAP = new Dictionary<(int, int), BasicPipe>();
    public static class Directions {
        // index: the index in which the connection state is saved in BasicPipe
        public static int RIGHT = 0, DOWN = 1, LEFT = 2, UP = 3, TOTAL_DIRECTIONS = 4;
        public static int getOpposite(int direction) {
            return (direction + 2) % TOTAL_DIRECTIONS;
        }
    }
    public class PipeTraceResult {
        public HashSet<BasicPipe> allPipes;
        public HashSet<BasicPipe> leakyPipes;
        private HashSet<BasicPipe> visited;
        // constructor
        public PipeTraceResult(BasicPipe startPipe) {
            startTracePipe(startPipe);
        }
        // clears info and traces the connected pipes from a starting point
        public void startTracePipe(BasicPipe startPipe) {
            allPipes = new HashSet<BasicPipe>();
            leakyPipes = new HashSet<BasicPipe>();
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
            (ArrayList, bool) queryResult = startPipe.getConnectedPipes();
            foreach (BasicPipe connectedPipe in queryResult.Item1) {
                traceSinglePipe(connectedPipe);
            }
            // record starting pipe if it is leaky
            if (queryResult.Item2)
                leakyPipes.Add(startPipe);
        }
    }
}
