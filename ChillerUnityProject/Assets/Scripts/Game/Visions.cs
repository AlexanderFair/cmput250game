using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
    public List<VisionStruct> visionsList;

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
        VisionPredicate predicate = (UIObjectClass.IsUIActive() ? VisionPredicate.PLAY_ON_UI : 0) | 
                                    (RoomObjectClass.CanUpdate() ? VisionPredicate.PLAY_ON_ROOM : 0);
        IEnumerable<VisionStruct> visions = visionsList.Where(x => (x.insanityLevel & Insanity.GetInsanityLevel()) != 0 && (x.predicate & predicate) != 0);

        Instantiate(Util.ChooseRandom(visions).vision);
    }

    [System.Serializable]
    public struct VisionStruct
    {
        public GameObject vision;
        public Insanity.Level insanityLevel;
        public VisionPredicate predicate;
    }

    [Flags]
    public enum VisionPredicate { PLAY_ON_UI = 1, PLAY_ON_ROOM = 2 }

}

