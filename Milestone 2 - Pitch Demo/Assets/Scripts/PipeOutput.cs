using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeOutput : BasicPipe
{
    public PipeOutput() {
        base.connectedDir = new bool[]{true, false, false, false};
    }
    // maze completed
    public void handleConnected() {
        base.handleConnected();
        // TODO: maybe change it to something else?
        Debug.Log("SOLVED!");
    }
    // sadly, such pipe can not be rotated :(
    public void OnMouseDown() {
    }
}
