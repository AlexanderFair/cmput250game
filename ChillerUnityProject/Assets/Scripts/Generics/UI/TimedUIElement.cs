using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//An object that lasts a certain amount of time before killing itself
public class TimedUIElement : UIObjectClass
{
    [Header("Timed UI Object")]
    public float aliveTime = 0;

    private float timer = 0;

    protected override void UpdateUIObject()
    {
        base.UpdateUIObject();
        timer += Time.deltaTime;
        if(timer > aliveTime)
        {
            DestroyUIObject(gameObject);
        }
    }
}
