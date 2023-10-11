using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeTriangular : BasicPipe
{
    public static int PIPE_TYPE_IDX = 5;

    public PipeTriangular() {
        base.connectedDir = new bool[]{true, true, true, false};
    }
    // Start is called before the first frame update
    void Start()
    {
        // call the start function from parent class
        base.Start();
        
        pipeTypeIndex = PIPE_TYPE_IDX;
    }
}
