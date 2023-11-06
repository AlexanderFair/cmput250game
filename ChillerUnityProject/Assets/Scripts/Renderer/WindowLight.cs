﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class WindowLight : RoomObjectClass
{
    private readonly static string
        LIGHT_COLOR = "_LightColor",
        FOCAL_POINT = "_FocalPoint",
        CENTRE_DIST = "_CentreToFocalDistance",
        CIRC_CENTRE = "_WindowCentre",
        CIRC_RADIUS = "_WindowRadius",
        OUTER_ANGLE = "_OuterAngle",
        INNER_ANGLE = "_InnerAngle",
        PITCH_ANGLE = "_PitchAngle",
        CUTOFF_DIST = "_CutoffDistance",
        FADING_DIST = "_MinCutoffDistance",
        FADE_Y_AXIS = "_CutoffUsingY";

    [ColorUsage(true, hdr: true)]
    public Color color;
    public float outerAngle;
    public float innerAngle;
    public float pitchAngle;
    public Vector2 windowCentre;
    public float windowRadius;
    public float cutoffDistance;
    public bool fadeAlongYAxis;

    public SpriteRenderer spriteRenderer;

    private Material mat;
    private Vector2 focal;

    public override void Start()
    {
        base.Start();
        mat = new Material(Settings.PrefabMaterials.WindowLight.Get());
        spriteRenderer.material = mat;
        UpdateShader();
    }

    public void UpdateShader()
    {
        UpdateFocal();
        UpdateCutoff();
        UpdateColor();
    }

    public override void Update()
    {
        UpdateShader();
    }

    public void UpdateColor()
    {
        mat.SetColor(LIGHT_COLOR, color);
    }

    public void UpdateFocal()
    {
        float delta = Mathf.Sqrt(2) * 0.5f * windowRadius * (1 + 1f / Mathf.Tan(innerAngle*Mathf.PI*2));
        Vector2 focalOffset = new Vector2(delta * Mathf.Sin(pitchAngle*Mathf.PI*2), -delta * Mathf.Cos(pitchAngle*Mathf.PI*2));
        focal = windowCentre + focalOffset;
        float fadingDist = delta + windowRadius;

        mat.SetFloat(CENTRE_DIST, (windowCentre-focal).magnitude);
        mat.SetVector(CIRC_CENTRE, windowCentre);
        mat.SetFloat(CIRC_RADIUS, windowRadius);
        mat.SetVector(FOCAL_POINT, focal);
        mat.SetFloat(INNER_ANGLE, innerAngle);
        mat.SetFloat(OUTER_ANGLE, outerAngle);
        mat.SetFloat(PITCH_ANGLE, pitchAngle);
        mat.SetFloat(FADING_DIST, fadingDist);
    }

    public void UpdateCutoff()
    {
        mat.SetFloat(CUTOFF_DIST, cutoffDistance);
        mat.SetInt(FADE_Y_AXIS, fadeAlongYAxis ? 1 : 0);
    }
}
