using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeTriangular : BasicPipe
{
    public static int PIPE_TYPE_IDX = 5;

    public PipeTriangular() {
        base.connectedDir = new bool[]{true, true, true, false};
    }
    
    protected override void initStats() {
        // call the initStats function from parent class
        base.initStats();
        
        pipeTypeIndex = PIPE_TYPE_IDX;
    }
}
