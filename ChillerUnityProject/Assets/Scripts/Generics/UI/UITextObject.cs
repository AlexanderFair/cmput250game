﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * A class for updateable text objects
 */
public class UITextObject : UIObjectClass
{
    [Header("UI Text Settings")]
    public Text textComponent;
    public string defaultValue;
    private string currentText = "";
    private string nextText = "";

    protected override void StartUIObject()
    {
        base.StartUIObject();
        SetText(defaultValue, true);
    }

    protected override void UpdateUIObject()
    {
        base.UpdateUIObject();
        SetCurrentText();
    }

    private void SetCurrentText()
    {
        if (!nextText.Equals(currentText))
        {
            textComponent.text = nextText.ToString();
            currentText = nextText;
        }
    }

    /*
     * Returns the current text that is displayed
     */
    public string GetCurrentText()
    {
        return currentText;
    }

    /*
     * Sets the displayed text on the next update call
     * 
     * If force is set, then the text is immediately displayed 
     */
    public void SetText(string _text, bool force = false)
    {
        nextText = _text;
        if (force)
        {
            SetCurrentText();
        }
    }

}
