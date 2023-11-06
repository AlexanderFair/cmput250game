using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineSpriteClass : MonoBehaviour
{
    private static readonly string
        INTENSITY_MIN_COLOR = "_IntensityMinColor",
        INTENSITY_MAX_COLOR = "_IntensityMaxColor",
        INTENSITY_INTERPILATION_FACTOR = "_IntensityInterpFactor",
        ALPHA_FACTOR = "_AlphaFactor";

    public SpriteRenderer spriteRenderer;
    private Material material;
    private readonly float enableTime = 0.5f; //TODO make setting
    private float cycleTime = 1f; //TODO make random

    //If the outline is visible
    public bool On { get; private set; } = false;
    //If the outline is set to be on
    public bool SetOn { get; private set; } = false;

    //Runs the code as an isolated system
    //This will cause the code to setup on Awake and update on Update
    public bool runIsolated = false;

    private float runningTime = 0f;
    private float lightingTime = 0f;

    public void Start()
    {
        if(runIsolated)
        {
            SetupOutline(spriteRenderer, (spriteRenderer.material.GetColor(INTENSITY_MIN_COLOR), spriteRenderer.material.GetColor(INTENSITY_MAX_COLOR)));
            TurnOn();
        }
    }

    public void Update()
    {
        if (runIsolated)
        {
            UpdateOutliner();
        }
    }

    public void SetupOutline(SpriteRenderer _spriteRenderer, (Color, Color) colours)
    {
        spriteRenderer = _spriteRenderer;
        if(spriteRenderer == null)
        {
            Settings.DisplayError("sprite Renderer is null", gameObject);
            DestroyImmediate(gameObject);
            return;
        }

        material = new Material(Settings.PrefabMaterials.Outline.Get());
        if(material == null)
        {
            Settings.DisplayError("material is null", gameObject);
            DestroyImmediate(gameObject);
            return;
        }
        spriteRenderer.material = material;
        material.SetColor(INTENSITY_MIN_COLOR, colours.Item1);
        material.SetColor(INTENSITY_MAX_COLOR, colours.Item2);
        material.SetFloat(INTENSITY_INTERPILATION_FACTOR, 0f);
        material.SetFloat(ALPHA_FACTOR, 0f);
    }

    public void UpdateOutliner()
    {
        if(On == SetOn && !SetOn)
        {
            return; 
        }

        Cycle();

        if(On != SetOn)
        {
            if (On) Darken(); else Lighten();
        }
        
    }

    private void Lighten()
    {
        lightingTime += Time.deltaTime;
        if (lightingTime >= enableTime)
        {
            lightingTime = enableTime;
            On = true;
        }
        material.SetFloat(ALPHA_FACTOR, Mathf.SmoothStep(0, 1, lightingTime / enableTime));
    }

    private void Darken()
    {
        lightingTime -= Time.deltaTime;
        if(lightingTime <= 0)
        {
            lightingTime = 0;
            On = false;
            runningTime = 0;
        }
        material.SetFloat(ALPHA_FACTOR, Mathf.SmoothStep(0,1,lightingTime / enableTime));
    }

    private void Cycle()
    {
        
        runningTime += Time.deltaTime;
        float cycleAmount = Mathf.Abs((runningTime % (cycleTime*4)) / (cycleTime*2) - 1f);
        material.SetFloat(INTENSITY_INTERPILATION_FACTOR, Mathf.SmoothStep(0, 1, cycleAmount));
    }

    public void TurnOn()
    {
        if (SetOn) return; // Already on
        if (On)
        {//We were darkening
            On = false; //To make sure we lighten up again
        }
        SetOn = true;
    }

    public void TurnOff()
    {
        if (!SetOn) return; // Already off
        if (!On)
        {//We were lightening
            On = true; //make sure we darken
        }
        SetOn = false;
    }
}
