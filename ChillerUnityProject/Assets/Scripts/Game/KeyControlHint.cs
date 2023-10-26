using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyControlHint : RoomObjectClass, Settings.ISettingsUpdateWatcher
{
    public Text text;
    //Use <key> for the keybind
    public string hint;
    public Collider2D colliderB;

    private Transform currentTransform = null;
    private Settings.Controls currentControl;
    private Vector2 currentOffset = Vector2.zero;
    private bool activeHinting = false;

    public override void Start()
    {
        base.Start();
        UpdateText();
    }

    protected override void UpdateRoomObject()
    {
        base.UpdateRoomObject();
        if(!activeHinting)
        {
            return;
        }

        UpdatePos();

        if (colliderB.Distance(Player.Instance.getCollider()).distance < 0)
        {
            text.gameObject.SetActive(false);
        }
        else
        {
            text.gameObject.SetActive(true);
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    public void Clear()
    {
        activeHinting = false;
        UpdateText();
    }

    public void SetToObject(Transform followposition, Settings.Controls control, Vector2 offset)
    {
        activeHinting = true;
        currentControl = control;
        currentTransform = followposition;
        currentOffset = offset;
        if(currentTransform == null)
        {
            Settings.DisplayError("transform is null", gameObject);
            throw new System.ArgumentException();
        }
        UpdateText();
        UpdatePos();
    }

    public void ControlsUpdated(Settings.Controls control)
    {
        if (!activeHinting)
        {
            return;
        }
        if (control == currentControl)
        {
            UpdateText();
        }
    }

    private void UpdatePos()
    {
        if (activeHinting)
        {
            transform.position = currentTransform.position + (Vector3)currentOffset;
        }
        else
        {
            transform.position = Vector3.zero;
        }
    }

    private void UpdateText()
    {
        if (!activeHinting)
        {
            text.text = "";
            return;
        }

        string s = hint.Replace("<key>", currentControl.GetKeyCode().ToString());
        text.text = s;
    }

    public void FloatValuesUpdated(Settings.FloatValues floatVal)
    { }

}
