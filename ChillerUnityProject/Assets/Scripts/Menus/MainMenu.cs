using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : GameMenuObject
{
    [Header("Main Menu Setting")]
    public GameObject cutscenePrefab;

    protected override void UpdateMenuObject() { }

    public override void OnMenuClick(MenuClickableObject obj)
    {
        if (obj.name == "cross")
        {
            Application.Quit();
        }
        else if(obj.name == "play")
        {
            MenuController.Instance.ExitMenu();
        }
        else if (obj.name == "settings")
        {
            MenuController.Instance.ChangeMenu(MenuController.Instance.settingsMenuPrefab);
        }
    }

    protected override void OnDestroyMenuObject()
    {
        base.OnDestroyMenuObject();
    }

    public override MenuController.MenuTransitionDirection TransitionDirection()
    {
        return MenuController.MenuTransitionDirection.Exit;
    }
}
