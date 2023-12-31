﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Interactor2 : MonoBehaviour
{
    private static readonly string
        INTERP_FACTOR = "_InterpFactor";

    public float totalTime = 1f;
    public float speed = 10f;

    private float timer = 0f;

    public SpriteRenderer spriteRenderer;
    private Material mat;

    void Awake()
    {
        mat = spriteRenderer.material;
        mat.SetFloat(INTERP_FACTOR, Factor());
    }

    private bool active = false;

    public void Update()
    {
        if(RoomObjectClass.CanUpdate() && Settings.Controls.Interact.GetKeyDown(false, false))
        {
            active = true;
            timer = 0f;
            spriteRenderer.enabled = true;
        }

        if (active)
        {
            if(timer > totalTime)
            {
                active = false;
                spriteRenderer.enabled = false;
            }
            mat.SetFloat(INTERP_FACTOR, Factor());
            timer += Time.deltaTime;
        }
    }

    private float Factor()
    {
        float per = timer / totalTime;
        per *= speed + 1;
        per -= 1;
        if(per > 0)
        {
            per *= 1f / speed;
        }
        else
        {
            per *= -1;
        }


        return Mathf.SmoothStep(0,1,per);
    }
}
