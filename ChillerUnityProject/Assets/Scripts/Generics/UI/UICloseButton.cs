using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICloseButton : ClickableUIObject
{
    [Header("UI Close Button")]
    public GameObject textHint;

    protected override void StartUIObject()
    {
        base.StartUIObject();
        //InstantiateUIElement(textHint).GetComponent<UICloseText>().Setup(this);
    }

    protected override void Clicked()
    {
        base.Clicked();
        clickControl.PlaySoundEffect();
        ClearUI();
    }
}
