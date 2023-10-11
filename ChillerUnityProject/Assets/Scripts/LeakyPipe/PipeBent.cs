using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeBent : BasicPipe
{
    public static int PIPE_TYPE_IDX = 4;

    public PipeBent() {
        base.connectedDir = new bool[]{true, true, false, false};
    }
    // Start is called before the first frame update
    void Start() {
        // call the start function from parent class
        base.Start();
        
        pipeTypeIndex = PIPE_TYPE_IDX;
    }
}
