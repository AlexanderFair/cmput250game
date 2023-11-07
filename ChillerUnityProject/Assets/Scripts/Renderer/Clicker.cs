using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicker : MonoBehaviour
{
    private static readonly string
        LOCATION = "_Location",
        INTERP_FACTOR = "_InterpFactor",
        INIT_COLOR = "_InitColor",
        END_COLOR = "_EndColor";

    public float totalTime = 1f;

    private float timer;

    public SpriteRenderer spriteRenderer;
    [ColorUsage(true, hdr: true)]
    public Color initColor;
    [ColorUsage(true, hdr: true)]
    public Color endColor;
    private Material mat;

    public void Setup(Vector2 location)
    {
        mat.SetVector(LOCATION, location);
    }

    void Awake()
    {
        mat = spriteRenderer.material;
        mat.SetFloat(INTERP_FACTOR, 0f);
        mat.SetColor(INIT_COLOR, initColor);
        mat.SetColor(END_COLOR, endColor);
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
