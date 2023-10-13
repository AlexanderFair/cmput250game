using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MenuClickableObject;

/*
 * Controls the menus in the game
 * 
 * To switch to a menu, call ChangeMenu(prefab) where prefab is the new menu object
 */
public class MenuController : MonoBehaviour
{
    public static MenuController Instance { get; private set; }
    private static bool isDefined = false;

    public GameObject mainMenuPrefab;
    public GameObject pauseMenuPrefab;
    public GameObject settingsMenuPrefab;


    private GameObject currentMenuPrefab = null;
    private GameMenuObject currentMenuObject = null;
    private GameObject previousMenuPrefab = null; 

    // Start is called before the first frame update
    void Start()
    {
        if(isDefined)
        {
            DestroyImmediate(gameObject);
            return;
        }
        isDefined = true;
        Instance = this;
        DontDestroyOnLoad(gameObject);
        ChangeMenu(mainMenuPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        if(UIObjectClass.IsUIActive() && Settings.Controls.UIExit.GetKeyDown())
        {
            UIObjectClass.ClearUI();
        }
        else if (Settings.Controls.MenuTransition.GetKeyDown())
        {
            ChangeMenu();
        }
    }

    /* Changes the menu to the next menu based off of the MenuTransition
     * given by the current menu
     *
     * Defaults to the pausemenu if no direction can be found
     */
    public void ChangeMenu(GameObject switchTo=null)
    {
        if(switchTo != null)
        {
            SwitchMenus(switchTo);
            return;
        }
        if(currentMenuObject != null)
        {
            switch (currentMenuObject.TransitionDirection())
            {
                case MenuTransitionDirection.Exit:
                    ExitMenu();
                    break;
                case MenuTransitionDirection.Previous:
                    GotoPreviousMenu();
                    break;
                default: break;
            }
        }
        else
        {
            SwitchMenus(pauseMenuPrefab);
        }
    }

    // Exits the menu
    public void ExitMenu()
    {
        currentMenuPrefab = null;
        previousMenuPrefab = null;
        currentMenuObject = null;
        MenuObjectClass.ClearMenu();
    }

    /* Goes to the previous menu
     * 
     * If the previous menu is null, it exits the menu
     */
    public void GotoPreviousMenu()
    {
        SwitchMenus(previousMenuPrefab);
    }

    /*
     * Switches to the prefab given
     * If the prefab is null, the menu is exited
     */
    private void SwitchMenus(GameObject switchToPrefab)
    {
        if(switchToPrefab == null)
        {
            ExitMenu();
            return;
        }

        GameMenuObject temp = MenuObjectClass.InstantiateMenuElement(switchToPrefab).GetComponent<GameMenuObject>();
        if (currentMenuObject != null)
        {
            MenuObjectClass.DestroyMenuObject(currentMenuObject.gameObject);
        }
        previousMenuPrefab = currentMenuPrefab;
        currentMenuPrefab = switchToPrefab;
        currentMenuObject = temp;
    }

    /* 
     * The next menu that should be switched to if the menutransition 
     * key is pressed on the current menu
     */
    public enum MenuTransitionDirection { 
        Exit, // Exits out of the menu
        Previous // Goes to the previous menu
    }
}

// Represents a pause menu or start menu
public abstract class GameMenuObject : MenuClickCaller
{
    /* 
     * The next menu that should be switched to if the menutransition 
     * key is pressed on the current menu
     */
    public abstract MenuController.MenuTransitionDirection TransitionDirection();
}