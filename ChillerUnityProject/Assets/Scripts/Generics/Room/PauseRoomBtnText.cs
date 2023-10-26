using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseRoomBtnText : RoomObjectClass, Settings.ISettingsUpdateWatcher
{

    [Header("Pause Room Btn Text")]
    public Text text;
    public string hint;

    private PauseRoomBtn btn;

    public void Setup(PauseRoomBtn b)
    {
        btn = b;
        this.AwakeSettingsWatcher();
    }

    public void ControlsUpdated(Settings.Controls control)
    {
        if (control == Settings.Controls.ExitMenu)
        {
            UpdateText();
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        this.DestroySettingsWatcher();
    }

    private void UpdateText()
    {
        string s = hint.Replace("<key>", Settings.Controls.ExitMenu.GetKeyCode().ToString());
        text.text = s;
    }

    public void FloatValuesUpdated(Settings.FloatValues floatVal)
    {
    }
}
