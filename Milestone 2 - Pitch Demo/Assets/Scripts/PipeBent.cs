using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeBent : BasicPipe
{
    public PipeBent() {
        base.connectedDir = new bool[]{true, true, false, false};
    }
}
