﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using static Settings;

/*
 *  The singleton settings instance which will be present in all scenes
 *  This contains the pairing of every setting and its respective value
 *  that can be changed during the game and initalized through the inspector
 *  
 *  Go through the dictionary to set a value
 */
public class SettingsInstance : MonoBehaviour
{
    // The settings instance
    public static SettingsInstance Instance { get; private set; }
    private static bool isDefined = false;

    [Header("Controls")]
    // The set of KeyCode pairings
    public KeyCodeValue[] controlsValues =
    {
        new KeyCodeValue { key = Controls.Interact, value = KeyCode.F },
        new KeyCodeValue { key = Controls.ExitMenu, value = KeyCode.Escape },
        new KeyCodeValue { key = Controls.Click, value = KeyCode.Mouse0 },
        new KeyCodeValue { key = Controls.RotatePipesLeft, value = KeyCode.Mouse0 },
        new KeyCodeValue { key = Controls.SkipDialogue, value = KeyCode.Return },
        new KeyCodeValue { key = Controls.Pause, value = KeyCode.P },
        new KeyCodeValue { key = Controls.MoveUp, value = KeyCode.W },
        new KeyCodeValue { key = Controls.MoveDown, value = KeyCode.S },
        new KeyCodeValue { key = Controls.MoveLeft, value = KeyCode.A },
        new KeyCodeValue { key = Controls.MoveRight, value = KeyCode.D },
        new KeyCodeValue { key = Controls.RotatePipesRight, value = KeyCode.Mouse1 }
    };
    //Played when Control.UseControl() is called
    //If multiple are set to the same control, a random one is choosen
    public ControlsSoundEffect[] controlSoundEffects = { };

    [Header("Floats")]
    // The set of float value pairings
    public FloatValue[] floatValues =
    {
        new FloatValue { key = FloatValues.PlayerInteractDistance, value = 1.0f },
        new FloatValue { key = FloatValues.DialogueSpeed, value = 32f },
        new FloatValue { key = FloatValues.DialogCompletionWaitForCloseSeconds, value = 5f },
        new FloatValue { key = FloatValues.FPS, value = 32f },
        new FloatValue { key = FloatValues.AmbientVolume, value = 1f },
        new FloatValue { key = FloatValues.SoundEffectVolume, value = 1f },
        new FloatValue { key = FloatValues.SoundtrackVolume, value = 1f }
    };

    [Header("Animations")]
    // The set of prefab animation pairings
    public PrefabAnimationValue[] prefabAnimValues = 
    { 
        new PrefabAnimationValue { key = PrefabAnimations.NullAnim, value = AnimationSpriteClass.NULL_STRUCT }
    };

    [Header("Prefab Objects")]
    // The set of prefab object pairings
    public PrefabObjectValue[] prefabObjectValues = { };

    [Header("Matereials")]
    //The set of prefab material pairings
    public PrefabMaterialValue[] prefabMaterialValues = { };

    [Header("Outlines")]
    //The set of outline pairings
    public OutlineValue[] outlineValues = { };

    [Header("Other")]
    public bool canLogWarnings = false;
    public int disabledInputCount = 0;
    public bool hideControlHintsWhenOverlappingPlayer = false;
    public AudioClip[] audioClips;

    //The list of interfaces which should get updated if a float value changes
    public List<ISettingsUpdateWatcher> settingsValueChangeWatchers = new List<ISettingsUpdateWatcher>();

    //Dictionaries generated by the value sets
    public Dictionary<Controls, KeyCodeValue> controlsPairings = new Dictionary<Controls, KeyCodeValue>();
    public Dictionary<FloatValues, FloatValue> floatPairings = new Dictionary<FloatValues, FloatValue>();
    public Dictionary<PrefabAnimations, PrefabAnimationValue> animPairings = new Dictionary<PrefabAnimations, PrefabAnimationValue>();
    public Dictionary<PrefabObjects, PrefabObjectValue> objectPairings = new Dictionary<PrefabObjects, PrefabObjectValue>();
    public Dictionary<PrefabMaterials, PrefabMaterialValue> materialPairings = new Dictionary<PrefabMaterials, PrefabMaterialValue>();
    public Dictionary<Outlines, OutlineValue> outlinePairings = new Dictionary<Outlines, OutlineValue>();
    //the set of last frame uses for each key
    public Dictionary<Controls, int> controlsLastUsedFrame = new Dictionary<Controls, int>();
    public Dictionary<Controls, List<AudioClip>> controlSoundEffectsDict = new Dictionary<Controls, List<AudioClip>>();

    void Awake()
    {
        if (isDefined)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
        isDefined = true;
        DontDestroyOnLoad(gameObject);

        //Set up dicitonaries
        foreach (KeyCodeValue v in controlsValues){controlsPairings[v.key] = v;}
        foreach (FloatValue v in floatValues) { floatPairings[v.key] = v; }
        foreach (PrefabAnimationValue v in prefabAnimValues) { animPairings[v.key] = v; }
        foreach (PrefabObjectValue v in prefabObjectValues) { objectPairings[v.key] = v; }
        foreach (PrefabMaterialValue v in prefabMaterialValues) { materialPairings[v.key] = v; }
        foreach (OutlineValue v in outlineValues) { outlinePairings[v.key] = v; }

        foreach (Controls c in Enum.GetValues(typeof(Controls)))
        {
            controlsLastUsedFrame[c] = -1;
            controlSoundEffectsDict[c] = new List<AudioClip>();
        }

        foreach (ControlsSoundEffect cs in controlSoundEffects)
        {
            controlSoundEffectsDict[cs.key].AddRange(cs.sounds);
        }
    }

    // Key value parings for each setting type
    [System.Serializable] public struct KeyCodeValue { public Controls key; public KeyCode value; }
    [System.Serializable] public struct ControlsSoundEffect { public Controls key; public List<AudioClip> sounds; }
    [System.Serializable] public struct FloatValue { public FloatValues key; public float value; }
    [System.Serializable] public struct PrefabAnimationValue { public PrefabAnimations key; public Sprite[] value; }
    [System.Serializable] public struct PrefabObjectValue { public PrefabObjects key; public GameObject value; }
    [System.Serializable] public struct PrefabMaterialValue { public PrefabMaterials key; public Material value; }
    [System.Serializable] public struct OutlineValue { 
        public Outlines key;
        [ColorUsage(true, hdr: true)]
        public Color minIntensity;
        [ColorUsage(true, hdr: true)]
        public Color maxIntensity;
        public bool circular;
    }

    public void UpdateSettingWatchers(object e)
    {
        foreach (ISettingsUpdateWatcher s in settingsValueChangeWatchers)
        {
            s.UpdateSetting(e);
        }
    }

}

/*
 * The accessor for the settings.
 * All code should access the respective setting through the enum
 */
public static class Settings
{

    // Key Code controls
    public enum Controls
    {
        Interact, Click, ExitMenu, RotatePipesLeft, SkipDialogue, Pause, MoveLeft, MoveRight, MoveUp, MoveDown, RotatePipesRight
    }

    // Float values
    public enum FloatValues
    {
        PlayerInteractDistance, DialogueSpeed, DialogCompletionWaitForCloseSeconds, FPS,
        AmbientVolume, SoundtrackVolume, SoundEffectVolume, MasterVolume,
        SecondsPerOutlineCycleMin, SecondsPerOutlineCycleMax, OutlineIlluminateTime
    }

    // Prefab animations
    public enum PrefabAnimations
    {
        NullAnim, Player, Narrator
    }
    // Prefab objects
    public enum PrefabObjects
    {
        DialogDisplay, BigDialogDisplay
    }

    // Prefab Materials
    public enum PrefabMaterials
    {
        Null, Outline, WindowLight, Vignette
    }

    public enum Outlines
    {
        Null, Click, Drag, Interact, Collision
    }

    //Returns the KeyCode associated with the Control
    public static KeyCode GetKeyCode(this Controls control)
    {
        RequireSettingsInstance();
        return SettingsInstance.Instance.controlsPairings[control].value;
    }

    // Returns Input.GetKey evaluated on the associatied KeyCode
    // If forceGetInput, returns if the key is pressed independent if Input is enabled
    // Returns false if ignoreIfUsed is flagged and the key has been used this frame
    public static bool GetKey(this Controls control, bool forceGetInput = false, bool ignoreIfUsed = true)
    {
        RequireSettingsInstance();
        return Input.GetKey(control.GetKeyCode()) && (IsInputEnabled() || forceGetInput) && (!ignoreIfUsed || !control.IsUsedThisFrame());
    }

    // Returns Input.GetKeyDown evaluated on the associatied KeyCode
    // If forceGetInput, returns if the key is pressed independent if Input is enabled
    // Returns false if ignoreIfUsed is flagged and the key has been used this frame
    public static bool GetKeyDown(this Controls control, bool forceGetInput = false, bool ignoreIfUsed = true)
    {
        RequireSettingsInstance();
        return Input.GetKeyDown(control.GetKeyCode()) && (IsInputEnabled() || forceGetInput) && (!ignoreIfUsed || !control.IsUsedThisFrame());
    }

    // Returns Input.GetKeyUp evaluated on the associatied KeyCode
    // If forceGetInput, returns if the key is pressed independent if Input is enabled
    // Returns false if ignoreIfUsed is flagged and the key has been used this frame
    public static bool GetKeyUp(this Controls control, bool forceGetInput = false, bool ignoreIfUsed = true)
    {
        RequireSettingsInstance();
        return Input.GetKeyUp(control.GetKeyCode()) && (IsInputEnabled() || forceGetInput) && (!ignoreIfUsed || !control.IsUsedThisFrame());
    }

    // returns if the control was used or not this frame
    public static bool IsUsedThisFrame(this Controls control)
    {
        return SettingsInstance.Instance.controlsLastUsedFrame[control] >= Time.frameCount;
    }
    // sets the control to have been used this frame
    // if playSoundEffect is set, plays a sound effect if there is one associated
    public static void UseControl(this Controls control, bool playSoundEffect = false)
    {
        SettingsInstance.Instance.controlsLastUsedFrame[control] = Time.frameCount;
        if (playSoundEffect)
        {
            control.PlaySoundEffect();
        }
    }

    //plays a random assiciated sound effect
    public static void PlaySoundEffect(this Controls control)
    {
        List<AudioClip> clipList = SettingsInstance.Instance.controlSoundEffectsDict[control];
        if (clipList.Count > 0)
        {
            AudioHandler.Instance.playSoundEffect(Util.ChooseRandom(clipList));
        }
    }



    //Sets the control value
    public static void Set(this Controls value, KeyCode f)
    {
        RequireSettingsInstance();
        SettingsInstance.KeyCodeValue v = SettingsInstance.Instance.controlsPairings[value];
        if (v.value != f)
        {
            SettingsInstance.Instance.controlsPairings[value] = new SettingsInstance.KeyCodeValue { key = v.key, value = f };
            SettingsInstance.Instance.UpdateSettingWatchers(value);
        }
    }


    //Returns the float associated with the FloatValue
    public static float Get(this FloatValues value)
    {
        RequireSettingsInstance();
        return SettingsInstance.Instance.floatPairings[value].value;
    }
    //Sets the float value
    public static void Set(this FloatValues value, float f)
    {
        RequireSettingsInstance();
        SettingsInstance.FloatValue v = SettingsInstance.Instance.floatPairings[value];
        if (v.value != f)
        {
            SettingsInstance.Instance.floatPairings[value] = new SettingsInstance.FloatValue { key = v.key, value = f };
            SettingsInstance.Instance.UpdateSettingWatchers(value);
        }
    }

    //Returns the associated prefab object
    public static GameObject Get(this PrefabObjects obj)
    {
        RequireSettingsInstance();
        return SettingsInstance.Instance.objectPairings[obj].value;
    }

    //Returns the associated prefab material
    public static Material Get(this PrefabMaterials obj)
    {
        RequireSettingsInstance();
        return SettingsInstance.Instance.materialPairings[obj].value;
    }

    //Returns the associated animation 
    public static Sprite[] Get(this PrefabAnimations obj)
    {
        RequireSettingsInstance();
        return SettingsInstance.Instance.animPairings[obj].value;
    }

    //Returns the color associated with the color
    public static (Color, Color, bool) Get(this Outlines outline)
    {
        RequireSettingsInstance();
        SettingsInstance.OutlineValue val = SettingsInstance.Instance.outlinePairings[outline];
        return (val.minIntensity, val.maxIntensity, val.circular);
    }

    private static void RequireSettingsInstance()
    {
        if(SettingsInstance.Instance == null)
        {
            // This can occur if the object calling is loaded before settings
            // And this is required in the load method
            throw new NullReferenceException("The Settings instance is not in the scene or requested before loaded");
        }
    }

    // Display Warning
    public static void DisplayWarning(string warning, GameObject fromObject)
    {
        if(SettingsInstance.Instance == null)
        {
            Debug.LogWarning(warning + " | Generated by gameObject : " + fromObject?.name);
        }
        else
        {
            RequireSettingsInstance();
            if (SettingsInstance.Instance.canLogWarnings)
            {
                Debug.LogWarning(warning + " | Generated by gameObject : " + fromObject?.name);
            }
        }
        
    } 

    // Displays Error
    public static void DisplayError(string error, GameObject fromObject)
    {
        if(SettingsInstance.Instance == null)
        {
            Debug.LogError(error + " | Generated by gameObject : " + fromObject?.name);
        }
        else
        {
            RequireSettingsInstance();
            Debug.LogError(error + " | Generated by gameObject : " + fromObject?.name);
        }
    }


    // Disables the ability for input to return true through settings methods
    public static void DisableInput()
    {
        SettingsInstance.Instance.disabledInputCount++;
    }

    // Enables the ability for input to return true through settings methods
    public static void EnableInput()
    {
        SettingsInstance.Instance.disabledInputCount = SettingsInstance.Instance.disabledInputCount <= 1 ? 0 : SettingsInstance.Instance.disabledInputCount - 1;
    }

    public static bool IsInputEnabled()
    {
        return SettingsInstance.Instance.disabledInputCount <= 0;
    }


    // Method is called if the setting is updated
    public interface ISettingsUpdateWatcher
    {

        void FloatValuesUpdated(FloatValues floatVal);
        void ControlsUpdated(Controls control);

    }

    public static void UpdateSetting(this ISettingsUpdateWatcher watcher, object e)
    {
        switch (e)
        {
            case FloatValues f:
                watcher.FloatValuesUpdated(f); break;
            case Controls c:
                watcher.ControlsUpdated(c); break;
            default:
                break;
        }
    }

    public static void AwakeSettingsWatcher(this ISettingsUpdateWatcher watcher)
    {
        RequireSettingsInstance();
        SettingsInstance.Instance.settingsValueChangeWatchers.Add(watcher);
    }

    public static void DestroySettingsWatcher(this ISettingsUpdateWatcher watcher)
    {
        if(SettingsInstance.Instance != null)
        {
            SettingsInstance.Instance.settingsValueChangeWatchers.Remove(watcher);
        }
    }

}
