using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicker : MonoBehaviour
{
    private static readonly string
        LOCATION = "_Location",
        INTERP_FACTOR = "_InterpFactor";

    public float totalTime = 1f;

    private float timer;

    public SpriteRenderer spriteRenderer;
    private Material mat;

    public void Setup(Vector2 location)
    {
        mat.SetVector(LOCATION, location);
    }

    void Awake()
    {
        mat = spriteRenderer.material;
        mat.SetFloat(INTERP_FACTOR, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > totalTime)
        {
            DestroyImmediate(gameObject);
            return;
        }

        mat.SetFloat(INTERP_FACTOR, Mathf.SmoothStep(0,1,timer / totalTime));
    }
}
