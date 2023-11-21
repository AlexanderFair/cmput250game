using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *
 * The input pipe.
 *
 * THIS IS IMPLEMENTED CONSIDERING THAT THE GAME CONTAINS ONLY ONE OF THEM!
 *
 */
public class PipeInput : BasicPipe
{
    // the sound to play once completed
    public AudioClip COMPLETION_SOUND;

    public static int PIPE_TYPE_IDX = 1;

    public PipeInput() {
        base.connectedDir = new bool[]{true, false, false, false};
    }
    
    protected override void initStats() {
        // call the initStats function from parent class
        base.initStats();
        
        pipeTypeIndex = PIPE_TYPE_IDX;
    }

    // Update is called once per frame
    protected override void UpdateUIObject()
    {
        base.UpdateUIObject();
        // if a flow upate is required
        if (PipeGrid.getPuzzle().pendingFlowUpdate) {
            // reset all pipes to disconnected state
            foreach (BasicPipe pipe in PipeGrid.getPuzzle().PIPE_MAP.Values) {
                pipe.resetBeforeConnectionCheck();
                pipe.preOperationTick();
            }
            // reset leak amount
            PipeGrid.getPuzzle().liquidDecrement = 0;


            if (PipeGrid.getPuzzle().liquidRemaining > 0) {
                // make sure the current pipe is always connected to the source, which is itself.
                handleConnected();
                // trace the results, recall that the connected pipes are automatically marked as connected.
                PipeGrid.PipeTraceResult result = new PipeGrid.PipeTraceResult(this);
            }

            // handle liquid loss
            PipeGrid.getPuzzle().liquidRemaining -= PipeGrid.getPuzzle().liquidDecrement;
            // if the liquid is depleted, make sure it does not go negative
            if (PipeGrid.getPuzzle().liquidRemaining <= 0) {
                PipeGrid.getPuzzle().liquidRemaining = 0;
            }
            // update all pipes' sprite
            foreach (BasicPipe pipe in PipeGrid.getPuzzle().PIPE_MAP.Values) {
                pipe.pipeFlowSpriteUpdate();
                pipe.postOperationTick();
            }

            // play a finished sound if puzzle is solved
            if (PipeGrid.getPuzzle().isSolved())
                AudioHandler.Instance.playSoundEffect(COMPLETION_SOUND);
            // record flow update as completed
            PipeGrid.getPuzzle().pendingFlowUpdate = false;
        }
    }
    // sadly, such pipe can not be rotated :(
    public void OnMouseOver() {
    }
}
