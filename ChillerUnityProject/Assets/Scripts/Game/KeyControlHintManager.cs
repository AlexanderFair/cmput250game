using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyControlHintManager : RoomObjectClass
{
    [Header("Key Control manager")]
    public GameObject textPrefab;
    public static KeyControlHintManager Instance { get; private set; }
    private static bool defined = false;

    private Dictionary<GameObject, KeyControlHint> keys = new Dictionary<GameObject, KeyControlHint>();

    public override void Start()
    {
        base.Start();
        if (defined)
        {
            Settings.DisplayError("an instance already exist", gameObject);
            DestroyImmediate(gameObject);
            return;
        }
        defined = true;
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void GiveObjectHint(GameObject o, Transform t, Settings.Controls c, Vector2 offset)
    {
        if (keys.ContainsKey(o) && keys[o] != null)
        {
            Settings.DisplayWarning("hint already has input", o);
            return;
        }
        GameObject obj = Instantiate(textPrefab);
        KeyControlHint hint = obj.GetComponent<KeyControlHint>();
        hint.SetToObject(t, c, offset);
        keys[o] = hint;
    }

    public void RemoveObjectHint(GameObject o)
    {
        if(!(keys.ContainsKey(o) && keys[o] != null))
        {
            return;
        }
        keys[o].Clear();
        Destroy(keys[o].gameObject);
        keys[o] = null;
        keys.Remove(o);
    }
}
