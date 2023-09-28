using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * MenuObjectClass : MonoBehaviour
 * 
 * Represents the base class to be used for any object visible at the Menu level.
 * Object in the Menu should extend this class instead of MonoBehaviour
 * 
 * This class controls when the update method is called. Only if the Menu is enabled,
 * any object extending this class will get updated.
 * 
 * Subclasses must implement the UpdateMenuObject. This method will get called each frame
 * if the Menu should be updated.
 *
 * If a subclass wishes to override the Awake or OnDestroy methods, they should override the
 * AwakeMenuObject and OnDestroyMenuObject methods
 *
 * To set the Menu active, use the static ClearMenu and InstantiateNewMenu methods.
 * The user is in charge of removing and adding the objects to the screen.
 * 
 */
public abstract class MenuObjectClass : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(IsMenuActive())
        {
            UpdateMenuObject();
        }
    }

    /* 
     * Called when the objcet is first created 
     * 
     * If the Menu is not enabled, this enables the Menu
     */
    void Awake()
    {
        currentMenuObjects.Add(gameObject);
        EnableMenu();

        AwakeMenuObject();
    }

    /* 
     * Called when the object is destroyed 
     * 
     * If the Menu is enabled and this is the only object, the Menu is disabled.
     */
    void OnDestory()
    {
        currentMenuObjects.Remove(gameObject);
        if ( currentMenuObjects.Count == 0)
        {
            DisableMenu();
        }

        OnDestroyMenuObject();
    }

    /* 
     * Called when the objcet is first created 
     */
    public abstract void AwakeMenuObject();

    /* 
     * Called when the object is destroyed 
     */
    public abstract void OnDestroyMenuObject();

    /*
     * Called each frame when the Menu is enabled
     */
    public abstract void UpdateMenuObject();



    /* The set of currently active MenuObjects */
    protected static HashSet<GameObject> currentMenuObjects = new HashSet<GameObject>();

    /* If the Menu is active or not */
    private static bool isMenuActive = false;

    /* Sets the Menu to be active 
     * This does not create/delete any objects nor make an objects visible/invisible
     */
    public static void EnableMenu() { isMenuActive = true; }

    /* Sets the Menu to not be active 
     * This does not create/delete any objects nor make an objects visible/invisible
     */
    public static void DisableMenu() { isMenuActive = false; }

    /* Returns true if the Menu is active */
    public static bool IsMenuActive() { return isMenuActive; }

    /*
     * Destorys all curently active MenuObjects and disables the Menu
     */
    public static void ClearMenu()
    {
        if(!IsMenuActive()) { return; }

        //clone the list so we hit errors when removing while iterating
        HashSet<GameObject> iter = new HashSet<GameObject>(currentMenuObjects); 

        foreach(GameObject menu in iter)
        {
            Destroy(menu);
        }
        DisableMenu(); //just in case, the Menu should already be disabled by this point
    }

    /*
     * Clears the current Menu if applicable.
     * Then instantiates the newMenuprefab object
     * 
     * This will enable the Menu with only the newMenuprefab object
     */
    public static void IntantiateNewMenu(GameObject newMenuprefab)
    {
        ClearMenu();
        Instantiate(newMenuprefab);
    }
}
