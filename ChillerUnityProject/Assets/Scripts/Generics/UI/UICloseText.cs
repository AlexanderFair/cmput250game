using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICloseText : UIObjectClass, Settings.ISettingsUpdateWatcher
{

    private UICloseButton btn;
    public Text text;
    //Use <key> for the control
    public string hint;

    public void Setup(UICloseButton b)
    {
        btn = b;
        UpdateText();
        this.AwakeSettingsWatcher();
    }

    public void ControlsUpdated(Settings.Controls control)
    {
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

    public void FloatValuesUpdated(Settings.FloatValues floatVal)
    {
    }

    protected override void OnDestroyUIObject()
    {
        base.OnDestroyUIObject();
        this.DestroySettingsWatcher();
    }
}
