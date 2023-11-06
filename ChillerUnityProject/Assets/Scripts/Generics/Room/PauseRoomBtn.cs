using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseRoomBtn : RoomObjectClass, IClickableSprite
{
    [Header("Close Button")]
    public SpriteRenderer clickableRenderer;
    public Collider2D clickCollider;
    public Settings.Controls clickControl;

    public GameObject textHint;

    public bool ClickableCondition()
    {
        return CanUpdate();
    }

    public override void Start()
    {
        base.Start();
        //Instantiate(textHint).GetComponent<PauseRoomBtnText>().Setup(this);
        this.AwakeOutlinableSprite(clickableRenderer);
    }

    protected override void UpdateRoomObject()
    {
        base.UpdateRoomObject();
        this.UpdateOutlinableSprite();
        if (Util.GetKeyDownWithMouseOverObject(clickControl, clickCollider) && ClickableCondition())
        {
            Click();
        }

    }

    private void Click()
    {
        clickControl.PlaySoundEffect();
        MenuController.Instance.ChangeMenu();
    }

}
