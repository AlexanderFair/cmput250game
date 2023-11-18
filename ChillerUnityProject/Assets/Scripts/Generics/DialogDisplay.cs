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
    public int characterLength;
    public bool Complete { set; get; } = false;

    // The profile animation
    public AnimationSpriteClass profileAnimator;
    public GameObject skipInstruct;
    // The text object
    private Text textObject = null;

    private char[] targetText;
    private int currentChar = 0;
    private string currentText="";
    private float currentTime = 0;
    private bool disableMovement = false;
    private bool disableInput = false;
    private int currentLineBreaks = 0;
    private int targetLineBreaks;

    //The list of current html structs that are in use
    private List<string> html = new List<string>();

    public void Start()
    {
        if(!disableInput)
        {
            DestroyImmediate(skipInstruct);
        }
    }

    // Update is called once per frame when the menu is not active
    void Update()
    {
        if (MenuObjectClass.IsMenuActive())
        {
            return;
        }

        currentTime += Time.deltaTime;

        bool skip = Settings.Controls.SkipDialog.GetKeyDown(forceGetInput: true);
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

        profileAnimator.UpdateAnimation();

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
        if (currentTime >= 1f / Settings.FloatValues.DialogueSpeed.Get())
        {
            AddText();
            SetText();
            currentTime -= 1f / Settings.FloatValues.DialogueSpeed.Get();

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
        if(currentChar >= targetText.Length)
        {
            return '\0';
        }
        char c = targetText[currentChar++];
        currentText += c;
        if (c == '\n')
        {
            currentLineBreaks += 1;
        }
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
        for (int i = currentLineBreaks; i < targetLineBreaks; i++)
        {
            text += '\n';
        }
        textObject.text = text;
    }

    //Ends the dialog and destroys the game object
    public void KillDialog()
    {
        if(disableInput)
        {
            if (disableMovement)
            {
                UIObjectClass.DisableUI();
            }
            Settings.EnableInput();
        }
        currentDisplay = null;
        Destroy(gameObject);
    }

    private void SetTargetText(string text, int lineBreaks = 0)
    {
        targetText = text.ToCharArray();
        targetLineBreaks = lineBreaks;
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
    public static void NewDialog(string _text,
                                 Sprite[] animationStruct,
                                 bool disableInput = false,
                                 bool big = false)
    {
        if (_text == null || _text.Length == 0)
        {
            Settings.DisplayWarning("text is empty", null);
            return;
        }
        StopCurrentDisplay();

        GameObject gobj = Instantiate(big ? Settings.PrefabObjects.BigDialogDisplay.Get() : Settings.PrefabObjects.DialogDisplay.Get());
        currentDisplay = gobj.GetComponent<DialogDisplay>();

        //Set the profile animation
        currentDisplay.profileAnimator.ChangeAnimation(animationStruct);
        currentDisplay.profileAnimator.StartAnimation();

        //Set text object
        currentDisplay.textObject = gobj.transform.Find("Canvas").Find("Text").GetComponent<Text>();

        //Set target text
        GenerateTargetCharSet(_text, currentDisplay);

        currentDisplay.disableInput = disableInput;

        if (disableInput)
        {
            Settings.DisableInput();
            if (!UIObjectClass.IsUIActive())
            {
                //currentDisplay.disableMovement = true;
                //UIObjectClass.EnableUI();
            }
        }
    }

    private static void GenerateTargetCharSet(string targetString, DialogDisplay display)
    {
        char[] chars = targetString.ToCharArray();
        int currentCharCount = 0;

        string currentString = "";

        int lastSpaceChar = 0;
        string currentWord = "";

        bool insideHtml = false;
        int lineBreaks = 0;
        
        for(int i=0; i < chars.Length; i++)
        {
            currentWord += chars[i];
            if (chars[i] == ' ')//TODO change to break char
            {
                currentString += currentWord;
                currentWord = "";
                lastSpaceChar = i;
            }
            if (chars[i] == '\n')
            {
                lineBreaks++;
            }
            if (chars[i] == '<')
            {
                insideHtml = true;
            }
            else if (insideHtml)
            {
                if (chars[i] == '>')
                {
                    insideHtml = false;
                }
            }
            else
            {
                currentCharCount++;
            }
            if(currentCharCount >= display.characterLength)
            {
                currentWord = "";
                i = lastSpaceChar;
                currentString += '\n';
                lineBreaks++;
                currentCharCount = 0;
            }
        }
        currentString += currentWord;
        display.SetTargetText(currentString, lineBreaks);

    }

    public static void NewDialog(string _text,
                                 Settings.PrefabAnimations anim,
                                 bool disableInput = false,
                                 bool big = false)
    {
        NewDialog(_text, anim.Get(), disableInput, big);
    }

    public static void NewDialog(DialogStruct dialogStruct)
    {
        NewDialog(dialogStruct.dialog, dialogStruct.animation, dialogStruct.disableInput, dialogStruct.big);
    }

    //Chooses a random struct out of the array if the array has 1 or more elements
    public static void NewDialog(DialogStruct[] dialogStructs)
    {
        if(dialogStructs.Length > 0)
        {
            NewDialog(Util.ChooseRandom(dialogStructs));
        }
    }

    //Chooses a random struct out of the list if the list has 1 or more elements
    public static void NewDialog(List<DialogStruct> dialogStructs)
    {
        if (dialogStructs.Count > 0)
        {
            NewDialog(Util.ChooseRandom(dialogStructs));
        }
    }

    [System.Serializable]
    public struct DialogStruct
    {
        public string dialog;
        public Settings.PrefabAnimations animation;
        public bool disableInput;
        public bool big;
    }
}
