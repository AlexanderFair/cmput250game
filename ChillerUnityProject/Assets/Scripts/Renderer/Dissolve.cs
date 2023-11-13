using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float time;

    private float currentValue = 1f;
    private Material material;

    public void Start()
    {
        material = spriteRenderer.material;
        material.SetFloat("_Fade", 1f);
    }

    private bool add = false;

    public void Update()
    {
        currentValue += Time.deltaTime / time * (add ? 1 : -1);
        if (currentValue >= 1f) { currentValue = 2f - currentValue; add = false; }
        if(currentValue <= 0f) { currentValue = -currentValue; add = true;}
        currentValue = Mathf.Clamp(currentValue, 0f, 1f);

        material.SetFloat("_Fade", currentValue);
    }
}
