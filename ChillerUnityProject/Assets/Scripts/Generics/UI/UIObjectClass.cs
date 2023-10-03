using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * UIObjectClass : MonoBehaviour
 * 
 * Represents the base class to be used for any object visible at the UI level.
 * Object in the ui should extend this class instead of MonoBehaviour for code purposes.
 * 
 * For any object that does not have a behaviour that should be added to the ui, instantiate it using the 
 * InstantiateUIObject or InstantiateNewUIObject methods.
 * 
 * This class controls when the update method is called. Only if the UI is active and the Menus are not, 
 * any object extending this class will get updated.
 * 
 * Subclasses must implement the UpdateUIObject. This method will get called each frame
 * if the ui should be updated.
 *
 * If a subclass wishes to override the Awake or OnDestroy methods, they should override the
 * AwakeUIObject and OnDestroyUIObject methods
 *
 * To set the UI active, use the static ClearUI and InstantiateNewUI methods.
 * The user is in charge of removing and adding the objects to the screen.
 * 
 */
public abstract class UIObjectClass : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(IsUIActive() && !MenuObjectClass.IsMenuActive())
        {
            UpdateUIObject();
        }
    }

    /* 
     * Called when the objcet is first created and adds it to the set of objects if not already done so
     * 
     * If the UI is not enabled, this enables the UI
     * 
     * Having this method defined ensures that any UIObject is in the list of current UIObjects
     */
    void Awake()
    {
        AddGameObject(gameObject);
        AwakeUIObject();
    }

    /* 
     * Called when the object is destroyed 
     * 
     * If the UI is enabled and this is the only object, the UI is disabled.
     */
    void OnDestroy()
    {
        RemoveGameObject(gameObject);
        OnDestroyUIObject();
    }

    /* 
     * Called when the objcet is first created 
     */
    protected virtual void AwakeUIObject() { }

    /* 
     * Called when the object is destroyed 
     */
    protected virtual void OnDestroyUIObject() { }

    /*
     * Called each frame when the UI is enabled and the Menu is not active
     */
    protected abstract void UpdateUIObject();



    /* The set of currently active uiObjects */
    protected static HashSet<GameObject> currentUIObjects = new HashSet<GameObject>();

    /* If the ui is active or not */
    private static bool isUIActive = false;

    /* Sets the ui to be active 
     * This does not create/delete any objects nor make an objects visible/invisible
     */
    public static void EnableUI() { isUIActive = true; }

    /* Sets the ui to not be active 
     * This does not create/delete any objects nor make an objects visible/invisible
     */
    public static void DisableUI() { isUIActive = false; }

    /* Returns true if the ui is active */
    public static bool IsUIActive() { return isUIActive; }

    /*
     * Destorys all curently active uiObjects and disables the UI
     */
    public static void ClearUI()
    {
        if(!IsUIActive()) { return; }

        //clone the list so we hit errors when removing while iterating
        HashSet<GameObject> iter = new HashSet<GameObject>(currentUIObjects); 

        foreach(GameObject ui in iter)
        {
            DestroyUIObject(ui);
        }
        DisableUI(); //just in case, the ui should already be disabled by this point
    }

    /*
     * Clears the current UI if applicable.
     * Then instantiates the newUIprefab object
     * 
     * This will enable the UI with only the newUIprefab object
     * 
     * Returns the new game object
     */
    public static GameObject InstantiateNewUIElement(GameObject newUIprefab)
    {
        ClearUI();
        return InstantiateUIElement(newUIprefab);
    }

    /*
     * Instantiates the new object and adds it to the list of this current uis objects
     * If no ui exists currenlty, a new ui is created.
     * 
     * Returns the new GameObject
     */
    public static GameObject InstantiateUIElement(GameObject uiPrefab)
    {
        GameObject t = Instantiate(uiPrefab);
        AddGameObject(t);
        return t;
    }

    public static void DestroyUIObject(GameObject uiObject)
    {
        RemoveGameObject(uiObject);
        Destroy(uiObject);
    }

    /*
     * Adds a game object to the list of current game objects
     * Enables the ui if the ui is not enabled
     */
    private static void AddGameObject(GameObject g)
    {
        currentUIObjects.Add(g);
        EnableUI();
    }

    /*
     * Removes a game object from the list of current game objects
     * If no more game objects exists, the ui is disabled.
     */
    private static void RemoveGameObject(GameObject g)
    {
        currentUIObjects.Remove(g);
        if (currentUIObjects.Count == 0)
        {
            DisableUI();
        }
    }
}
