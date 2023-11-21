using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LiquidRemainingText : UIObjectClass
{
    // update the message displayed on awake
    void Start() {
        UpdateUIObject();
    }
    protected override void StartUIObject() {
    }
    protected override void OnDestroyUIObject() {
    }

    // Update is called once per frame
    protected override void UpdateUIObject() {
        Text attatchedObj = this.gameObject.GetComponent<Text>();
        int liquidDecrement = PipeGrid.getPuzzle().liquidDecrement;
        attatchedObj.text = "Amount of Water Remaining: " + PipeGrid.getPuzzle().liquidRemaining + 
                        (liquidDecrement > 0 ? " (Water depletion: " + liquidDecrement + "/turn)" : "");
    }
}
