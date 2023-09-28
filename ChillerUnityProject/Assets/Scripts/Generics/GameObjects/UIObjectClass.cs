using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * UIObjectClass : MonoBehaviour
 * 
 * Represents the base class to be used for any object visible at the UI level.
 * Object in the ui should extend this class instead of MonoBehaviour
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
     * Called when the objcet is first created 
     * 
     * If the UI is not enabled, this enables the UI
     */
    void Awake()
    {
        currentUIObjects.Add(gameObject);
        EnableUI();

        AwakeUIObject();
    }

    /* 
     * Called when the object is destroyed 
     * 
     * If the UI is enabled and this is the only object, the UI is disabled.
     */
    void OnDestory()
    {
        currentUIObjects.Remove(gameObject);
        if ( currentUIObjects.Count == 0)
        {
            DisableUI();
        }

        OnDestroyUIObject();
    }

    /* 
     * Called when the objcet is first created 
     */
    public abstract void AwakeUIObject();

    /* 
     * Called when the object is destroyed 
     */
    public abstract void OnDestroyUIObject();

    /*
     * Called each frame when the UI is enabled and the Menu is not active
     */
    public abstract void UpdateUIObject();



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
            Destroy(ui);
        }
        DisableUI(); //just in case, the ui should already be disabled by this point
    }

    /*
     * Clears the current UI if applicable.
     * Then instantiates the newUIprefab object
     * 
     * This will enable the UI with only the newUIprefab object
     */
    public static void IntantiateNewUI(GameObject newUIprefab)
    {
        ClearUI();
        Instantiate(newUIprefab);
    }
}
