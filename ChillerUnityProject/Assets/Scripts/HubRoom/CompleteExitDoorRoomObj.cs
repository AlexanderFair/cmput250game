using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteExitDoorRoomObj : DisplayUIRoomObject
{
    protected override void Interact()
    {
        base.Interact();
        if (Insanity.Instance.IsHigh())
        {
            GameManager.Instance.StartSwitchScene("InsaneCutscene", Vector2.zero);
        }
        else if(CageRoomScript.unlocked)
        {
            GameManager.Instance.StartSwitchScene("SaneCutscenePenguin", Vector2.zero);
        }
        else
        {
            GameManager.Instance.StartSwitchScene("SaneCutsceneNoPenguin", Vector2.zero);
        }
    }
}
