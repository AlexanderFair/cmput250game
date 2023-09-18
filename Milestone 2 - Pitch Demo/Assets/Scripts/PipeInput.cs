using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeInput : BasicPipe
{
    public PipeInput() {
        base.connectedDir = new bool[]{true, false, false, false};
    }
    // Start is called before the first frame update
    void Start()
    {
        // call the start function from parent class
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        // reset all pipes to disconnected state
        foreach (BasicPipe pipe in PipeGrid.PIPE_MAP.Values)
            pipe.resetBeforeConnectionCheck();
        // the current pipe is always connected to the source, which is itself.
        handleConnected();
        // trace the results, recall that the connected pipes are automatically marked as connected.
        PipeGrid.PipeTraceResult result = new PipeGrid.PipeTraceResult(this);
    }
    // sadly, such pipe can not be rotated :(
    public void OnMouseDown() {
    }
}
