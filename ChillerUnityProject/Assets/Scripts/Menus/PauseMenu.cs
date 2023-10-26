using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : GameMenuObject
{

    protected override void UpdateMenuObject() { }

    public override void OnMenuClick(MenuClickableObject obj)
    {
        if(obj.name == "MenuCloseBtn" || obj.name == "exit")
        {
            MenuController.Instance.ExitMenu();
        }
        else if (obj.name == "settings")
        {
            MenuController.Instance.ChangeMenu(MenuController.Instance.settingsMenuPrefab);
        }
    }

    public override MenuController.MenuTransitionDirection TransitionDirection()
    {
        return MenuController.MenuTransitionDirection.Exit;
    }
}
