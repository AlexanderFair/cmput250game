using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicPipe : MonoBehaviour
{
    public static SpriteRenderer LEAK_SPRITE_RENDERER_TEMPLATE;
    public Sprite EMPTY_SPRITE, CONNECTED_SPRITE;
    // Direction: starts with right (index of 0, see PipeGrid.class)
    // each rotation will be clockwise, similar to direction index
    public bool[] connectedDir, leakDir = new bool[]{false, false, false, false};
    public bool isConnected = false;
    // sprites for leak
    public Dictionary<int, SpriteRenderer> leakRenderers = new Dictionary<int, SpriteRenderer>();
    // upper left: x = 0, y = 0
    // right: x+   up: y+
    public int gridX, gridY, rotation;
    public BasicPipe getRelativePipe(int direction) {
        (int, int) targetCoord;
        // should have used switch, but this is not an enum so if-else statement is utilized
        if (direction == PipeGrid.Directions.RIGHT)
            targetCoord = (gridX + 1, gridY);
        else if (direction == PipeGrid.Directions.DOWN)
            targetCoord = (gridX, gridY - 1);
        else if (direction == PipeGrid.Directions.LEFT)
            targetCoord = (gridX - 1, gridY);
        else if (direction == PipeGrid.Directions.UP)
            targetCoord = (gridX, gridY + 1);
        else {
            Debug.Log("BasicPipe.getRelativePipe error: unknown direction " + direction);
            return null;
        }
        return PipeGrid.PIPE_MAP.ContainsKey(targetCoord) ? PipeGrid.PIPE_MAP[targetCoord] : null;
    }
    public void resetBeforeConnectionCheck() {
        isConnected = false;
        leakDir = new bool[]{false, false, false, false};
    }
    public bool isConnectedTowardsDir(int targetDirection) {
        int originalCorrespondingRotation = targetDirection - rotation + PipeGrid.Directions.TOTAL_DIRECTIONS;
        // add another total_directions before modulo it, in case of unexpected behaviour on mod operation with negative number
        originalCorrespondingRotation %= PipeGrid.Directions.TOTAL_DIRECTIONS;
        return connectedDir[originalCorrespondingRotation];
    }
    public void handleConnected() {
        isConnected = true;
    }
    public void handleLeak(int leakDirection) {
        leakDir[leakDirection] = true;
    }
    // return value: properly connected pipes in an array list and a boolean for whether the pipe is leaking out
    public (ArrayList, bool) getConnectedPipes() {
        bool isLeaking = false;
        ArrayList results = new ArrayList();
        // loop through all directions
        for (int targetDir = 0; targetDir < PipeGrid.Directions.TOTAL_DIRECTIONS; targetDir ++) {
            // if the curent pipe is connected towards this direction
            if (isConnectedTowardsDir(targetDir)) {
                // if the targeted pipe is also connected towards this direction
                BasicPipe nextPipe = getRelativePipe(targetDir);
                int oppositeDir = PipeGrid.Directions.getOpposite(targetDir);
                if (nextPipe != null && nextPipe.isConnectedTowardsDir(oppositeDir)) {
                    results.Add(nextPipe);
                    nextPipe.handleConnected();
                }
                // if the targeted pipe is not connected, display some water leaking out from this direction
                else {
                    isLeaking = true;
                    handleLeak(targetDir);
                }
            }
        }
        return (results, isLeaking);
    }
    public SpriteRenderer getAttatchedRenderer() {
        SpriteRenderer attatchedObj = this.gameObject.GetComponent<SpriteRenderer>();
        return attatchedObj;
    }
    // called once on start
    public void Start() {
        // generate grid x and y based on coords
        SpriteRenderer attatchedObj = getAttatchedRenderer();
        gridX = (int) ((this.transform.position.x + 1e-5) / attatchedObj.size.x);
        gridY = (int) ((this.transform.position.y + 1e-5) / attatchedObj.size.y);
        (int, int) coord = (gridX, gridY);
        // load current pipe into pipe grid
        // logs an error message when two pipes are at the same position
        if (PipeGrid.PIPE_MAP.ContainsKey(coord))
            Debug.Log("Two pipes are at the same position? Coord: " + coord);
        else 
            PipeGrid.PIPE_MAP.Add(coord, this);
    }
    public void handleSpriteRotation(Transform spriteInfo, int rotDir) {
        spriteInfo.rotation = new Quaternion(0f, 0f, 0f, 0f);
        spriteInfo.Rotate(0f, 0f, -90f * rotDir);
    }
    public void renderLeak(int direction) {
        // create sprite
        SpriteRenderer createdLeakDisplay = (SpriteRenderer) Object.Instantiate(LEAK_SPRITE_RENDERER_TEMPLATE);
        // modify sprite info
        float spriteX = this.transform.position.x, spriteY = this.transform.position.y;
        SpriteRenderer attatchedObj = getAttatchedRenderer();
        // should have used switch, but this is not an enum so if-else statement is utilized
        if (direction == PipeGrid.Directions.RIGHT)
            spriteX += (createdLeakDisplay.size.x + attatchedObj.size.x) / 2;
        else if (direction == PipeGrid.Directions.DOWN)
            spriteY -= (createdLeakDisplay.size.y + attatchedObj.size.y) / 2;
        else if (direction == PipeGrid.Directions.LEFT)
            spriteX -= (createdLeakDisplay.size.x + attatchedObj.size.x) / 2;
        else if (direction == PipeGrid.Directions.UP)
            spriteY += (createdLeakDisplay.size.y + attatchedObj.size.y) / 2;
        else {
            Debug.Log("BasicPipe.getRelativePipe error: unknown direction " + direction);
            return;
        }
        createdLeakDisplay.transform.position = new Vector3(spriteX, spriteY, 0);
        handleSpriteRotation(createdLeakDisplay.transform, PipeGrid.Directions.getOpposite(direction));
        createdLeakDisplay.enabled = true;
        createdLeakDisplay.name = "LEAK_SPRITE_" + this.gameObject.name + "_" + direction;
        // cache the sprite
        leakRenderers.Add(direction, createdLeakDisplay);
    }
    // update
    public void Update() {
        // update pipe sprite
        SpriteRenderer renderer = getAttatchedRenderer();
        renderer.sprite = isConnected ? CONNECTED_SPRITE : EMPTY_SPRITE;
        // update rotation
        handleSpriteRotation(this.transform, rotation);
        // update leak sprites
        // make leaks invisible by default
        foreach (SpriteRenderer leakSpriteRenderer in leakRenderers.Values) {
            leakSpriteRenderer.enabled = false;
        }
        // if the pipe is connected, make the proper leaking locations visible
        if (isConnected) {
            for (int validationDir = 0; validationDir < leakDir.Length; validationDir ++) {
                // if a leak is present on this direction
                if (leakDir[validationDir]) {
                    // a leak is already rendered? make it visible
                    if (leakRenderers.ContainsKey(validationDir))
                        leakRenderers[validationDir].enabled = true;
                    // otherwise, render a new leak
                    else
                        renderLeak(validationDir);
                }
            }
        }
    }
    // rotate when clicked
    public void OnMouseDown() {
        rotation = (rotation + 1) % PipeGrid.Directions.TOTAL_DIRECTIONS;
    }
}
