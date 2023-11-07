using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public GameObject clickObject;
    public GameObject dragObject;

    // Update is called once per frame

    public float waitForDragTime;
    public float dragIntervalTimeMoving;
    public float dragIntervalTimeWaiting;
    public float stillThreshold;
    private float dragTimer = 0f;

    private float stillTime = 0f;
    private Vector2 prevPos = Vector2.zero;

    void Update()
    {
        if(Settings.Controls.Click.GetKeyDown(true, false))
        {
            Instantiate(clickObject).GetComponent<Clicker>().Setup(Util.GetMouseWorldPoint());
        }

        if(Settings.Controls.Click.GetKey(false, false))
        {
            Vector2 p = Util.GetMouseWorldPoint();
            if(p == prevPos)
            {
                stillTime += Time.deltaTime;
            }
            else
            {
                stillTime = 0;
            }
            prevPos = p;
            dragTimer += Time.deltaTime;
            if(dragTimer > waitForDragTime)
            {

                if (dragTimer > waitForDragTime + (stillTime >= stillThreshold ? dragIntervalTimeWaiting : dragIntervalTimeMoving))
                {
                    dragTimer = waitForDragTime;
                    Instantiate(dragObject).GetComponent<Clicker>().Setup(Util.GetMouseWorldPoint());
                }
            }
        }
        else
        {
            dragTimer = 0f;
        }
    }
}
