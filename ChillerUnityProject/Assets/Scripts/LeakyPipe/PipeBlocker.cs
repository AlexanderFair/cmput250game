using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeBlocker : BasicPipe
{
    public static int PIPE_TYPE_IDX = 6;

    public PipeBlocker() {
        base.connectedDir = new bool[]{true, false, false, false};
    }
    
    protected override void initStats() {
        // call the initStats function from parent class
        base.initStats();
        
        pipeTypeIndex = PIPE_TYPE_IDX;
    }
}
