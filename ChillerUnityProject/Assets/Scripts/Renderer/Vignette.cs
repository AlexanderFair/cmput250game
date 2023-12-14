using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Vignette : MonoBehaviour
{
    private static readonly string
        ALPHA_STRENGTH = "_AlphaStrength";

    public float addTime;
    private float addTimer;
    private bool add = false;
    
    public SpriteRenderer spriteRenderer;
    private Material mat;

    void Start()
    {
        mat = new Material(Settings.PrefabMaterials.Vignette.Get());
        spriteRenderer.material = mat;
        mat.SetFloat(ALPHA_STRENGTH, 0);
    }

    public void Update()
    {
        float value = Mathf.SmoothStep(0, 1, Mathf.Clamp01(Insanity.Instance.GetInsanity() / 100f));
        if (add || addTimer > 0)
        {
            if (add)
            {
                addTimer = 0;
                add = false;
            }
            addTimer += Time.deltaTime;
            if(addTimer >= addTime)
            {
                addTimer = 0;
            }
        }
        float val = Mathf.Lerp(value, 1, Mathf.SmoothStep(0,1,1-Mathf.Abs(addTimer/addTime*2-1)));
        mat.SetFloat(ALPHA_STRENGTH, val);
    }

    public void Add()
    {
        add = true;
    }
}
