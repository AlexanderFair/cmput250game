using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//An object that lasts a certain amount of time before killing itself
public class TimedFadeUIElement : UIObjectClass
{
    [Header("Timed UI Object")]
    public float fadeTime = 0;

    private float timer = 0;

    public SpriteRenderer render;

    protected override void UpdateUIObject()
    {
        base.UpdateUIObject();
        timer += Time.deltaTime;
        if(timer < fadeTime)
        {
            render.color = new Color(render.color.r, render.color.g, render.color.b, Mathf.SmoothStep(0,1,timer/fadeTime));
        }
        else
        {
            render.color = new Color(render.color.r, render.color.g, render.color.b, 1f);
            enabled = false;
        }
    }
}
