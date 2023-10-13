﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class RandomNoteUIObject : UIObjectClass
{

    [Header("Random Note UI Settings")]
    // Use '<word0>', '<word1>', ... for the replacement words
    // i.e. "There exists a <word0> world of <word1>!"
    public string note;
    //array of comma sperated words
    //Each i-th element of the array will correspond with <wordi>
    public string[] commaSeperatedWordLists;

    public Text textElement;

    protected override void AwakeUIObject()
    {
        base.AwakeUIObject();
        string[][] words = new string[commaSeperatedWordLists.Length][];

        string noteCopy = note;
        System.Random rand = new System.Random();

        for (int i=0; i<commaSeperatedWordLists.Length; i++)
        {
            words[i] = commaSeperatedWordLists[i].Split(new string[] {", "}, System.StringSplitOptions.RemoveEmptyEntries);

            noteCopy = Regex.Replace(noteCopy, "<word" + i + ">", m => words[i][rand.Next(words[i].Length)]);
        }

        textElement.text = noteCopy;
        Insanity.Add(1);
        
    }


}
