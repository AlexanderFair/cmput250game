using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * The system which displays dialog
 * 
 * Call NewDialog to display a new dialog with a profil pic
 */
public class DialogDisplay : MonoBehaviour
{
    public bool Complete { set; get; } = false;

    // The profile animation
    private GameObject profilePicGameObject = null;
    // The text object
    private Text textObject = null;

    private char[] targetText;
    private int currentChar = 0;
    private string currentText="";
    private float currentTime = 0;

    //The list of current html structs that are in use
    private List<string> html = new List<string>();

    // Update is called once per frame when the menu is not active
    void Update()
    {
        if (MenuObjectClass.IsMenuActive())
        {
            return;
        }

        currentTime += Time.deltaTime;

        bool skip = Settings.Controls.SkipDialog.GetKeyDown();
        if(skip && Complete )
        {
            KillDialog();
        }
        else if(skip)
        {
            currentText = new string(targetText);
            textObject.text = currentText;
            currentTime = 0;
            Complete = true;
        }

        if (Complete)
        {
            CompleteUpdate();
        }
        else
        {
            IncompleteUpdate();
        }
        
    }

    // Updates when Complete is true
    private void CompleteUpdate()
    {
        if(currentTime >= Settings.FloatValues.DialogCompletionWaitForCloseSeconds.Get())
        {
            KillDialog();
        }
    }

    // Updates when Complete is false
    private void IncompleteUpdate()
    {
        if (currentTime >= 1f / Settings.FloatValues.DialogCharactersPerSecond.Get())
        {
            AddText();
            SetText();
            currentTime -= 1f / Settings.FloatValues.DialogCharactersPerSecond.Get();

            if (currentChar >= targetText.Length)
            {
                Complete = true;
            }
        }
    }

    /*
     * Adds a new text character to the currentText
     * If the next character is an html struct, the whole
     * struct is added and AddText() is called again
     * 
     * If the html struct is a beginning struct, the html is 
     * added to the list of current html structs so that the ending
     * can be added on to the text element
     */
    private void AddText()
    {
        char nextChar = GetChar();
        if (nextChar == '<')
        {
            bool equaled = false;
            bool backed = false;
            string htmls = "";
            char a = GetChar();
            if (a == '/')
            {
                backed = true;
                a = GetChar();
            }
            while (a != '>')
            {
                if (a == '=')
                {
                    equaled = true;
                }
                if (!equaled)
                {
                    htmls += a;
                }
                a = GetChar();
            }
            if (backed)
            {
                html.Remove(htmls);
            }else
            {
                html.Insert(0, htmls);
            }
            AddText();
        }
    }

    /*
     * Gets the current char and increase the counter
     *Adds the char to the current text
     */
    private char GetChar()
    {
        char c = targetText[currentChar++];
        currentText += c;
        return c;
    }

    // Sets the text and add any needed html structs
    private void SetText()
    {
        string text = currentText;
        foreach (string h in html)
        {
            text += "</" + h + ">";
        }
        textObject.text = text;
    }

    //Ends the dialog and destroys the game object
    public void KillDialog()
    {
        currentDisplay = null;
        Destroy(gameObject);
    }

    // The current displaying dialog
    private static DialogDisplay currentDisplay = null;

    // The current displaying dialog
    public static DialogDisplay CurrentDisplay { get { return currentDisplay; } }

    // Stops and destroys the currently playing dialog if one is playing
    public static void StopCurrentDisplay()
    {
        currentDisplay?.KillDialog();
    }

    // Clears the current dialog display and displays the new dialog with the profilePic
    public static void NewDialog(string _text, Sprite[] animationStruct)
    {
        if(_text.Length == 0)
        {
            Settings.DisplayWarning("text is empty", null);
            return;
        }
        StopCurrentDisplay();

        GameObject gobj = Instantiate(Settings.PrefabObjects.DialogDisplay.Get(), new Vector2(0,Settings.FloatValues.DialogDisplayYLocation.Get()), Quaternion.identity);
        currentDisplay = gobj.GetComponent<DialogDisplay>();

        //Set the profile animation
        currentDisplay.profilePicGameObject = gobj.transform.Find("ProfileBackground").Find("ProfilePic").gameObject;
        currentDisplay.profilePicGameObject.GetComponent<AnimationSpriteClass>().ChangeAnimation(animationStruct);

        //Set text object
        currentDisplay.textObject = gobj.transform.Find("Canvas").Find("Text").GetComponent<Text>();

        //Set target text
        currentDisplay.targetText = _text.ToCharArray();


    }
}
