using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeOutput : BasicPipe
{
    public static int PIPE_TYPE_IDX = 2;

    public PipeOutput() {
        base.connectedDir = new bool[]{true, false, false, false};
    }
    // Start is called before the first frame update
    void Start()
    {
        // call the start function from parent class
        base.Start();
        
        pipeTypeIndex = PIPE_TYPE_IDX;
    }
    // maze completed
    public void handleConnected() {
        base.handleConnected();
        // TODO: maybe change it to something else?
        Debug.Log("SOLVED!");
    }
    // sadly, such pipe can not be rotated :(
    public void OnMouseOver() {
    }
}
