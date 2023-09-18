using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeTriangular : BasicPipe
{
    public PipeTriangular() {
        base.connectedDir = new bool[]{true, true, true, false};
    }
}
