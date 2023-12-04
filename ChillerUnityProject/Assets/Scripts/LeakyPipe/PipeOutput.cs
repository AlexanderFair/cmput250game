using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeOutput : BasicPipe
{
    public static int PIPE_TYPE_IDX = 2;

    public PipeOutput() {
        base.connectedDir = new bool[]{true, false, false, false};
    }
    
    protected override void initStats() {
        // call the initStats function from parent class
        base.initStats();
        
        pipeTypeIndex = PIPE_TYPE_IDX;
    }
    // maze completed
    public void handleConnected() {
        base.handleConnected();
        // TODO: maybe change it to something else?
        Debug.Log("SOLVED!");
    }
    // sadly, such pipe can not be rotated :(
    public override void handleRotationAttempt() {
    }
}
