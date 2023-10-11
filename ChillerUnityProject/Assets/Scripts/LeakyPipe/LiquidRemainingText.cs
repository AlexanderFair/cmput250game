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
    protected override void AwakeUIObject() {
    }
    protected override void OnDestroyUIObject() {
    }

    // Update is called once per frame
    protected override void UpdateUIObject() {
        Text attatchedObj = this.gameObject.GetComponent<Text>();
        int liquidDecrement = PipeGrid.getPuzzle().liquidDecrement;
        attatchedObj.text = "Units of Liquid Remaining: " + PipeGrid.getPuzzle().liquidRemaining + 
                        (liquidDecrement > 0 ? " (liquid depletion rate: " + liquidDecrement + "/operation)" : "");
    }
}
