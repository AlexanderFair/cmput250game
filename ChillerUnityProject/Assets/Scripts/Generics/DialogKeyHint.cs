using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogKeyHint : RoomObjectClass
{
    [Header("Room move text info")]
    public string hint;
    public Text text;
    private DialogTextSettingsWatcher controlChangeWatcher;

    public override void Start()
    {
        base.Start();
        UpdateText();
        controlChangeWatcher = new DialogTextSettingsWatcher(this);
        controlChangeWatcher.AwakeSettingsWatcher();
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        controlChangeWatcher.DestroySettingsWatcher();
    }
    protected override void UpdateRoomObject()
    {
    }

    public void UpdateText()
    {
        string h = hint.Replace("<key>", Settings.Controls.SkipDialog.GetKeyCode().ToString());
        text.text = h;
    }
}

public class DialogTextSettingsWatcher : Settings.ISettingsUpdateWatcher
{
    private DialogKeyHint move;

    public DialogTextSettingsWatcher(DialogKeyHint diag)
    {
        this.move = diag;
    }


    public void ControlsUpdated(Settings.Controls control)
    {
        switch (control)
        {
            case Settings.Controls.SkipDialog:
                move.UpdateText();
                break;
            default:
                break;
        }
    }

    public void FloatValuesUpdated(Settings.FloatValues floatVal)
    {
    }
}
