using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseRoomBtnText : Settings.SettingsUpdateWatcher
{

    [Header("Pause Room Btn Text")]
    public Text text;
    public string hint;

    private PauseRoomBtn btn;

    public void Setup(PauseRoomBtn b)
    {
        btn = b;
    }

    public override void ControlsUpdated(Settings.Controls control)
    {
        base.ControlsUpdated(control);
        if (control == Settings.Controls.ExitMenu)
        {
            UpdateText();
        }
    }

    private void UpdateText()
    {
        string s = hint.Replace("<key>", Settings.Controls.ExitMenu.GetKeyCode().ToString());
        text.text = s;
    }

}
