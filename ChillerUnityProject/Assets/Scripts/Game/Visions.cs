using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/* 
 * Plays a vision
 * 
 * If the ui is currently enabled, a ui vision is instantiated to the ui
 * If the ui is not enabled, a sound effect is player
 */
public class Visions : MonoBehaviour
{

    public GameObject[] visions;
    public AudioClip[] sounds;

    private System.Random random = new System.Random();

    private static Visions _instance;
    private static bool isDefined = false;
    public static Visions Instance
    {
        get
        {
            if (_instance == null)
            {
                throw new System.NullReferenceException("There is no visions instance in the scene");
            }
            return _instance;
        }
    }

    void Start()
    {
        if (isDefined)
        {
            DestroyImmediate(gameObject);
            return;
        }
        isDefined = true;

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void HaveVision()
    {
        if (UIObjectClass.IsUIActive())
        {
            UIVision();
        }
        else
        {
            AudioVision();
        }
    }

    private void UIVision()
    {
        UIObjectClass.InstantiateUIElement(visions[random.Next(visions.Length)]);
    }

    private void AudioVision()
    {
        AudioHandler.Instance.playSoundEffect(sounds[random.Next(sounds.Length)]);
    }



}
