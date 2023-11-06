using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * MenuObjectClass : MonoBehaviour
 * 
 * Represents the base class to be used for any object visible at the Menu level.
 * Object in the Menu should extend this class instead of MonoBehaviour for code purposes.
 * 
 * For any object that does not have a behaviour that should be added to the Menu, instantiate it using the 
 * InstantiateMenuObject or InstantiateNewMenuObject methods.
 * 
 * This class controls when the update method is called. Only if the Menu is active and the Menus are not, 
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
public class MenuObjectClass : MonoBehaviour
{
    // Animations
    [Header("Menu Base Object Settings")]
    public List<AnimationSpriteClass> spriteAnimations = new List<AnimationSpriteClass>();

    // Update is called once per frame
    void Update()
    {
        if (IsMenuActive())
        {
            UpdateMenuObject();
            foreach(var anim in spriteAnimations)
            {
                anim?.UpdateAnimation();
            }
        }
    }

    /* 
     * Called when the objcet is first created and adds it to the set of objects if not already done so
     * 
     * If the Menu is not enabled, this enables the Menu
     * 
     * Having this method defined ensures that any MenuObject is in the list of current MenuObjects
     */
    void Start()
    {
        AddGameObject(gameObject);
        StartMenuObject();
        foreach (var anim in spriteAnimations)
        {
            anim?.AwakeAnimation();
        }
    }

    /* 
     * Called when the object is destroyed 
     * 
     * If the Menu is enabled and this is the only object, the Menu is disabled.
     */
    void OnDestroy()
    {
        foreach (var anim in spriteAnimations)
        {
            anim?.PauseAnimation();
        }
        RemoveGameObject(gameObject);
        OnDestroyMenuObject();
        
    }

    /* 
     * Called when the objcet is first created 
     */
    protected virtual void StartMenuObject() { }

    /* 
     * Called when the object is destroyed 
     */
    protected virtual void OnDestroyMenuObject() { }

    /*
     * Called each frame when the Menu is enabled and the Menu is not active
     */
    protected virtual void UpdateMenuObject() { }



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
        if (!IsMenuActive()) { return; }

        //clone the list so we hit errors when removing while iterating
        HashSet<GameObject> iter = new HashSet<GameObject>(currentMenuObjects);

        foreach (GameObject Menu in iter)
        {
            DestroyMenuObject(Menu);
        }
        DisableMenu(); //just in case, the Menu should already be disabled by this point
    }

    /*
     * Clears the current Menu if applicable.
     * Then instantiates the newMenuprefab object
     * 
     * This will enable the Menu with only the newMenuprefab object
     * 
     * Returns the new game object
     */
    public static GameObject InstantiateNewMenuElement(GameObject newMenuprefab)
    {
        ClearMenu();
        return InstantiateMenuElement(newMenuprefab);
    }

    /*
     * Instantiates the new object and adds it to the list of this current Menus objects
     * If no Menu exists currenlty, a new Menu is created.
     * 
     * Returns the new GameObject
     */
    public static GameObject InstantiateMenuElement(GameObject MenuPrefab)
    {
        GameObject t = Instantiate(MenuPrefab);
        AddGameObject(t);
        return t;
    }

    public static void DestroyMenuObject(GameObject MenuObject)
    {
        RemoveGameObject(MenuObject);
        Destroy(MenuObject);
    }

    /*
     * Adds a game object to the list of current game objects
     * Enables the Menu if the Menu is not enabled
     */
    protected static void AddGameObject(GameObject g)
    {
        currentMenuObjects.Add(g);
        EnableMenu();
    }

    /*
     * Removes a game object from the list of current game objects
     * If no more game objects exists, the Menu is disabled.
     */
    protected static void RemoveGameObject(GameObject g)
    {
        currentMenuObjects.Remove(g);
        if (currentMenuObjects.Count == 0)
        {
            DisableMenu();
        }
    }
}
