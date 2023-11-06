using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Class for a combination lock with variable combination options and input sections
 * 
 * Each instance of a combination lock must be given a list of input sections and a list of combination options
 * 
 * The input sections must optionaly contain a button to move the selction up and a button to move the selection down
 * The input section must contain a default selection value, and a location to appear the selection
 */
public class CombinationUIObject : UIObjectClass
{
    [Header("Combination UI Settings")]
    // The different options
    public GameObject[] optionPrefabs;
    // The input sections for the code
    public CombinationInputSection[] inputSections;


    private GameObject[] displayObjects;
    protected int[] values;
    //If the combination can be changed
    public bool CombinationActive { get; set; } = true;

    protected override void StartUIObject()
    {
        base.StartUIObject();
        values = new int[inputSections.Length];
        displayObjects = new GameObject[inputSections.Length];
        for (int i = 0; i < inputSections.Length; i++)
        {
            inputSections[i].changeUpButton?.Setup(i, true);
            inputSections[i].changeDownButton?.Setup(i, false);
            values[i] = inputSections[i].defaultValue;
            UpdateOptions(i, false);
        }

    }

    // Should be called by the selection buttons to notify the lock that the selection has changed
    public void ClickCall(CombinationInputButtonUIObject comboBtn)
    {
        values[comboBtn.Index] += comboBtn.IsUp ? 1 : optionPrefabs.Length-1;
        values[comboBtn.Index] %= optionPrefabs.Length;
        UpdateOptions(comboBtn.Index);
    }

    protected void UpdateOptions(int index, bool awake = true)
    {
        if (displayObjects[index] != null)
        {
            DestroyUIObject(displayObjects[index]);
        }
        displayObjects[index] = InstantiateUIElement(optionPrefabs[values[index]]);
        displayObjects[index].transform.position = inputSections[index].sectionLocation;
        if(awake)CodeUpdated();
    }

    protected virtual void CodeUpdated() { }
}

[System.Serializable]
/*
 * The struct of a combination input section
 * 
 * The input sections must optionaly contain a button to move the selction up and a button to move the selection down
 * The input section must contain a default selection value, and a location to appear the selection
 */
public struct CombinationInputSection
{
    public int defaultValue;
    public Vector3 sectionLocation;
    public CombinationInputButtonUIObject changeUpButton;
    public CombinationInputButtonUIObject changeDownButton;
}
