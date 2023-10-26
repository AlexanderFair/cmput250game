using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoomMoveTextSettingsWatcher : Settings.SettingsUpdateWatcher
{
    private RoomMoveTextInstructObject move;

    public void Setup(RoomMoveTextInstructObject _t)
    {
        move = _t;
    }

    public override void ControlsUpdated(Settings.Controls control)
    {
        base.ControlsUpdated(control);
        switch (control)
        {
            case Settings.Controls.MoveUp:
            case Settings.Controls.MoveRight:
            case Settings.Controls.MoveLeft:
            case Settings.Controls.MoveDown:
                move.UpdateText();
                break;
            default:
                break;
        }
    }
}
