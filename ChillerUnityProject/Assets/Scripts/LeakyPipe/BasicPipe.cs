using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 
 * This is the base class for all pipes in the pipe puzzle.
 * 
 * generally, it is sufficient to inherit this class then specify(override) the connected directions.
 * 
 */
public class BasicPipe : UIObjectClass
{
    public static GameObject LEAK_SPRITE_RENDERER_TEMPLATE;
    // pipe type index is a number uniquely determined by the pipe type: input, output etc. each have their own index.
    // this index is currently used to determine which set of sprite it should use.
    public int pipeTypeIndex;
    public static Dictionary<int, Sprite> CONNECTED_SPRITE = new Dictionary<int, Sprite>();
    public static Dictionary<int, Sprite[]> FROZEN_SPRITES = new Dictionary<int, Sprite[]>();

    // Direction: starts with right (index of 0, see PipeGrid.class)
    // each rotation will be clockwise, similar to direction index
    public bool[] connectedDir, leakDir = new bool[]{false, false, false, false};
    public bool isConnected = false;

    // sprites for leak
    public Dictionary<int, SpriteRenderer> leakRenderers = new Dictionary<int, SpriteRenderer>();

    // how frozen is the pipe? 0 is not frozen, MAX_FROZEN_LAYERS is for the most frozen state.
    public static int MAX_FROZEN_LAYERS = 3, MAX_MOBILE_FROZEN_LAYER = 2;
    // should the pipe be slowly frozen again if it is not connected to the source?
    public static bool FREEZE_ON_DISCONNECT = true;
    public int frozenState = MAX_FROZEN_LAYERS;

    // upper left: x = 0, y = 0
    // right: x+   up: y+
    // targetRotationDir: 0 is for not rotating, 1 is for CW, -1 for CCW
    public int gridX, gridY, rotation, targetRotationDir = 0;
    public float rotationProgress = 0;


    //
    // FUNCTIONS
    //

    //
    // BELOW: FUNCTIONS RELATED TO WATER/FROZEN CHECK
    //

    // initilaizes stats on awake
    public void initStats() {
        SpriteRenderer attatchedSpriteRenderer = getAttatchedRenderer();
        // generate rotation based on component rotation
        double compRot = this.transform.rotation.eulerAngles.z;
        if (compRot < 0)
            compRot += 90 * Math.Ceiling(compRot / -90);
        rotation = ((int) (4 - (compRot / 90) )) % 4;
        // generate grid x and y based on coords
        gridX = (int) Mathf.Round((float) ((attatchedSpriteRenderer.bounds.center.x) / (attatchedSpriteRenderer.bounds.size.x)) );
        gridY = (int) Mathf.Round((float) ((attatchedSpriteRenderer.bounds.center.y) / (attatchedSpriteRenderer.bounds.size.y)) );
        (int, int) coord = (gridX, gridY);
        // load current pipe into pipe grid
        // logs an error message when two pipes are at the same position
        if (PipeGrid.getPuzzle().PIPE_MAP.ContainsKey(coord)) {
            Debug.Log("Two pipes are at the same position? Coord: " + coord + "||" + this.transform.position);
            Destroy(this);
        }
        else 
            PipeGrid.getPuzzle().PIPE_MAP.Add(coord, this);
    }
    // if this is buggy, try let the other function run initStats()
    public void Start() {
        initStats();
    }
    protected override void AwakeUIObject() {
    }
    // the two functions below should be called when a pipe is rotating. In another word, this is the pipe's behaviour over "time", which is rotation operations.
    public void preOperationTick() {
        if (!isConnected && FREEZE_ON_DISCONNECT) {
            frozenState ++;
            if (frozenState > MAX_FROZEN_LAYERS)
                frozenState = MAX_FROZEN_LAYERS;
        }
    }
    // if the pipe is connected after the connection check ("post"OperationTick), the ice in it should melt.
    public void postOperationTick() {
        if (isConnected)
            frozenState = 0;
    }
    // get the relative pipe in the given direction
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
        return PipeGrid.getPuzzle().PIPE_MAP.ContainsKey(targetCoord) ? PipeGrid.getPuzzle().PIPE_MAP[targetCoord] : null;
    }
    // reset pipe stats before a new connection check loop
    public void resetBeforeConnectionCheck() {
        isConnected = false;
        leakDir = new bool[]{false, false, false, false};
    }
    // checks if the CURRENT pipe is attempting to connect to a given direction
    // note that this function accounts for the pipe rotation
    public bool isConnectedTowardsDir(int targetDirection) {
        // add another total_directions before modulo it, in case of unexpected behaviour on mod operation with negative number
        int originalCorrespondingRotation = targetDirection - rotation + PipeGrid.Directions.TOTAL_DIRECTIONS;
        originalCorrespondingRotation %= PipeGrid.Directions.TOTAL_DIRECTIONS;
        return connectedDir[originalCorrespondingRotation];
    }
    // the function called when a pipe is considered connected
    public void handleConnected() {
        isConnected = true;
    }
    // the function to CACHE the leaking information
    // THIS function has nothing to do with leak sprite display
    public void handleLeak(int leakDirection) {
        leakDir[leakDirection] = true;
        PipeGrid.getPuzzle().liquidDecrement ++;
    }
    // this is called when recursively tracing through all connected pipes
    // return value: properly connected pipes in an array list
    public ArrayList getConnectedPipes() {
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
                    handleLeak(targetDir);
                }
            }
        }
        return results;
    }
    // the getters to get an attatched component
    public SpriteRenderer getAttatchedRenderer() {
        SpriteRenderer attatchedObj = this.gameObject.GetComponent<SpriteRenderer>();
        return attatchedObj;
    }
    public BoxCollider2D getAttatchedCollider() {
        BoxCollider2D collider = this.gameObject.GetComponent<BoxCollider2D>();
        return collider;
    }

    // 
    // ROTATION AND SPRITE RENDERING
    //

    // this accounts for the sprite rotation
    // spriteInfo would be XXX.transform
    // spriteRotation: current rotation
    // targetRotDir: +1, -1 (0 works too, then it is stationary)
    // rotationInterpolate: the rotation progress, [0f, 1f]
    public void handleSpriteRotation(Transform spriteInfo, int spriteRotation, int targetRotDir, float rotationInterpolate) {
        spriteInfo.rotation = new Quaternion(0f, 0f, 0f, 0f);
        spriteInfo.Rotate(0f, 0f, -90f * (spriteRotation + (targetRotDir * rotationInterpolate) ) );
    }
    // THIS is the function that RENDERS leak according to CACHED information.
    public void renderLeak(int direction) {
        // create sprite
        GameObject createdGameObj = Instantiate(LEAK_SPRITE_RENDERER_TEMPLATE);
        SpriteRenderer createdLeakDisplay = createdGameObj.GetComponent<SpriteRenderer>();
        SpriteRenderer attatchedRenderer = getAttatchedRenderer();
        // modify sprite scale
        createdLeakDisplay.transform.localScale = attatchedRenderer.transform.localScale;
        // modify sprite position
        float spriteX = this.transform.position.x, spriteY = this.transform.position.y;
        // should have used switch, but this is not an enum so if-else statement is utilized
        if (direction == PipeGrid.Directions.RIGHT)
            spriteX += (createdLeakDisplay.size.x + attatchedRenderer.size.x) * attatchedRenderer.transform.localScale.x / 2;
        else if (direction == PipeGrid.Directions.DOWN)
            spriteY -= (createdLeakDisplay.size.y + attatchedRenderer.size.y) * attatchedRenderer.transform.localScale.y / 2;
        else if (direction == PipeGrid.Directions.LEFT)
            spriteX -= (createdLeakDisplay.size.x + attatchedRenderer.size.x) * attatchedRenderer.transform.localScale.x / 2;
        else if (direction == PipeGrid.Directions.UP)
            spriteY += (createdLeakDisplay.size.y + attatchedRenderer.size.y) * attatchedRenderer.transform.localScale.y / 2;
        else {
            Debug.Log("BasicPipe.getRelativePipe error: unknown direction " + direction);
            return;
        }
        createdLeakDisplay.transform.SetParent(PipeGrid.currUI.transform);
        createdLeakDisplay.transform.position = new Vector3(spriteX, spriteY, 0);
        handleSpriteRotation(createdLeakDisplay.transform, PipeGrid.Directions.getOpposite(direction), 0, 0f);
        createdLeakDisplay.enabled = true;
        createdLeakDisplay.name = "LEAK_SPRITE_" + this.gameObject.name + "_" + direction;
        // cache the sprite
        leakRenderers.Add(direction, createdLeakDisplay);
    }
    // update sprite rotation and visible leak sprites
    public void pipeFlowSpriteUpdate() {
        // update pipe sprite
        SpriteRenderer renderer = getAttatchedRenderer();
        renderer.sprite = FROZEN_SPRITES[pipeTypeIndex][frozenState];
        if (isConnected)
            renderer.sprite = CONNECTED_SPRITE[pipeTypeIndex];
        // update rotation
        handleSpriteRotation(this.transform, rotation, targetRotationDir, rotationProgress);

        // update leak sprites
        // make leaks invisible by default
        foreach (SpriteRenderer leakSpriteRenderer in leakRenderers.Values) {
            leakSpriteRenderer.enabled = false;
        }
        // if the pipe is connected and liquid is not yet depleted, make the proper leaking locations visible
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
    // tick rotation
    protected override void UpdateUIObject() {
        // if the pipe is currently rotating
        if (targetRotationDir != 0) {
            rotationProgress += 10 * Time.deltaTime;
            // if the rotation is finished
            if (rotationProgress > 1) {
                // add an additional PipeGrid.Directions.TOTAL_DIRECTIONS to prevent unexpected modular behaviour
                rotation = (rotation + targetRotationDir + PipeGrid.Directions.TOTAL_DIRECTIONS) % PipeGrid.Directions.TOTAL_DIRECTIONS;
                // reset rotation direction info
                targetRotationDir = 0;
                PipeGrid.getPuzzle().isRotating = false;
                PipeGrid.getPuzzle().pendingFlowUpdate = true;
            }
            // update rotation
            handleSpriteRotation(this.transform, rotation, targetRotationDir, rotationProgress);
        }
    }
    // attempt to initialize rotating attempt when clicked
    public void OnMouseOver() {
        // only if no grid is currently rotating
        if (PipeGrid.getPuzzle().isRotating)
            return;
        // if the pipe is frozen, play some frozen sound or verbal hint
        if (frozenState > MAX_MOBILE_FROZEN_LAYER) {
        }
        // handle rotation
        else {
            // initialize rotation direction
            bool isAttemptingToRotate = false;
            // left click, rotate clockwise
            if (Settings.Controls.RotatePipesRight.GetKeyDown()) {
                targetRotationDir = 1;
                isAttemptingToRotate = true;
            }
            // right click, rotate counter clockwise
            else if (Settings.Controls.RotatePipesLeft.GetKeyDown()) {
                targetRotationDir = -1;
                isAttemptingToRotate = true;
            }
            // if a left/right click is made, further setup rotation variables
            if (isAttemptingToRotate) {
                PipeGrid.getPuzzle().isRotating = true;
                rotationProgress = 0;
            }
        }
    }
    
    // overrides to the parent class, nothing necessary yet.
    protected override void OnDestroyUIObject() {
    }
}
