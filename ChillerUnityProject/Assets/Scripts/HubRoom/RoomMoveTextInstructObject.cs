using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomMoveTextInstructObject : RoomObjectClass
{
    [Header("Room move text info")]
    public string hint;
    public Text text;
    public int hideCount;
    private RoomMoveTextSettingsWatcher controlChangeWatcher;

    private int count = 0;

    public override void Start()
    {
        if (CrowbarRoomScript.HasCrowbar)
        {
            DestroyImmediate(gameObject);
            return;
        }
        base.Start();
        UpdateText();
        controlChangeWatcher = new RoomMoveTextSettingsWatcher(this);
        controlChangeWatcher.AwakeSettingsWatcher();
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        controlChangeWatcher.DestroySettingsWatcher();
    }
    protected override void UpdateRoomObject()
    {
        base.UpdateRoomObject();
        if(Settings.Controls.MoveUp.GetKeyDown(ignoreIfUsed: false) ||
           Settings.Controls.MoveDown.GetKeyDown(ignoreIfUsed: false) ||
           Settings.Controls.MoveRight.GetKeyDown(ignoreIfUsed: false) ||
           Settings.Controls.MoveLeft.GetKeyDown(ignoreIfUsed: false))
        {
            count++;
        }

        if(count >= hideCount)
        {
            Destroy(gameObject); return;
        }
    }

    public void UpdateText()
    {
        string h = hint.Replace("<keyn>", Settings.Controls.MoveUp.GetKeyCode().ToString())
                       .Replace("<keyw>", Settings.Controls.MoveLeft.GetKeyCode().ToString())
                       .Replace("<keys>", Settings.Controls.MoveDown.GetKeyCode().ToString())
                       .Replace("<keyl>", Settings.Controls.MoveRight.GetKeyCode().ToString())
                       .Replace("<keyp>", Settings.Controls.Pause.GetKeyCode().ToString())
                       .Replace("<keyq>", Settings.Controls.ExitMenu.GetKeyCode().ToString());
        text.text = h;
    }
}
