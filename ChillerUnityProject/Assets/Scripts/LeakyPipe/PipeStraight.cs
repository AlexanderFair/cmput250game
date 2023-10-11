using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeStraight : BasicPipe
{
    public static int PIPE_TYPE_IDX = 3;

    public PipeStraight() {
        base.connectedDir = new bool[]{true, false, true, false};
    }
    // Start is called before the first frame update
    void Start()
    {
        // call the start function from parent class
        base.Start();
        
        pipeTypeIndex = PIPE_TYPE_IDX;
    }
}
