using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeStraight : BasicPipe
{
    public static int PIPE_TYPE_IDX = 3;

    public PipeStraight() {
        base.connectedDir = new bool[]{true, false, true, false};
    }
    
    protected override void initStats() {
        // call the initStats function from parent class
        base.initStats();
        
        pipeTypeIndex = PIPE_TYPE_IDX;
    }
}
