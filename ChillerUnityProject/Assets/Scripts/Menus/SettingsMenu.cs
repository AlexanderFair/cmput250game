using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : GameMenuObject
{
    protected override void UpdateMenuObject() { }

    public override void OnMenuClick(MenuClickableObject obj)
    {
        if (obj.name == "cross")
        {
            MenuController.Instance.ChangeMenu();
        }
    }

    public override MenuController.MenuTransitionDirection TransitionDirection()
    {
        return MenuController.MenuTransitionDirection.Previous;
    }
}
