using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeStraight : BasicPipe
{
    public PipeStraight() {
        base.connectedDir = new bool[]{true, false, true, false};
    }
}
