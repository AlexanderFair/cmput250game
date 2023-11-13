using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColorAdjustmentPostProcessing : MonoBehaviour
{
    public static ColorAdjustmentPostProcessing Instance;

    public Volume volume;
    private ColorAdjustments postShader;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        volume.profile.TryGet(out postShader);
    }

    public void ChangeSaturation(float val)
    {
        postShader.saturation.value = val;
    }
}
