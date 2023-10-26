using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICloseText : Settings.SettingsUpdateWatcher
{

    private UICloseButton btn;
    public Text text;
    //Use <key> for the control
    public string hint;

    public void Setup(UICloseButton b)
    {
        btn = b;
        UpdateText();
    }

    public override void ControlsUpdated(Settings.Controls control)
    {
        base.ControlsUpdated(control);
        if(control == Settings.Controls.ExitMenu)
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
