using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoomMoveTextSettingsWatcher : Settings.ISettingsUpdateWatcher
{
    private RoomMoveTextInstructObject move;

    public RoomMoveTextSettingsWatcher(RoomMoveTextInstructObject roomMoveTextInstructObject)
    {
        this.move = roomMoveTextInstructObject;
    }


    public void ControlsUpdated(Settings.Controls control)
    {
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

    public void FloatValuesUpdated(Settings.FloatValues floatVal)
    {
    }
}
