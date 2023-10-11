using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A button which can change the selection of a combination lock
 */
public class CombinationInputButtonUIObject : ClickableUIObject
{
    [Header("Combination Input Button Settings")]
    //The combination lock
    public CombinationUIObject combinationLock;

    // The index of the respective input section in the lock
    public int Index { get; private set; }
    // True if the button should move the selection up
    public bool IsUp { get; private set; }

    // Sets up the variables for the button
    public void Setup(int _index, bool _up)
    {
        Index = _index;
        IsUp = _up;
    }

    protected override void Clicked()
    {
        combinationLock.ClickCall(this);
    }
}
