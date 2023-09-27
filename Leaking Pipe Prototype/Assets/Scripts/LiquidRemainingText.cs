using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LiquidRemainingText : MonoBehaviour
{
    void Start()
    {
        // update the message displayed
        Update();
    }

    // Update is called once per frame
    void Update()
    {
        Text attatchedObj = this.gameObject.GetComponent<Text>();
        attatchedObj.text = "Units of Liquid Remaining: " + PipeGrid.liquidRemaining + 
                        (PipeGrid.liquidDecrement > 0 ? " (liquid depletion rate: " + PipeGrid.liquidDecrement + "/operation)" : "");
    }
}
