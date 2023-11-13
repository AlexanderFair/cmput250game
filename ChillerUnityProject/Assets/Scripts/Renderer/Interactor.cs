using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : OutlineSpriteClass
{
    private float timer;

    public new void Update()
    {
        base.Update();
        timer += Time.deltaTime;
        if (timer > cycleTime)
        {
            DestroyImmediate(gameObject);
            return;
        }
        material.SetFloat(ALPHA_FACTOR, 1 - Mathf.Abs((2 * timer / cycleTime) - 1));
    }
}
