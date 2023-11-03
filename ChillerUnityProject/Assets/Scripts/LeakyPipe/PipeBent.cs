using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeBent : BasicPipe
{
    public static int PIPE_TYPE_IDX = 4;

    public PipeBent() {
        base.connectedDir = new bool[]{true, true, false, false};
    }
    
    protected override void initStats() {
        // call the initStats function from parent class
        base.initStats();
        
        pipeTypeIndex = PIPE_TYPE_IDX;
    }
}
