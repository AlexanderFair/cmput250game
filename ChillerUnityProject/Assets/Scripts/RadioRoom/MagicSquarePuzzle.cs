using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MagicSquarePuzzle : SnapManagerUIObject
{
    [Header("Magic puzzle settings")]
    public Text[] texts;
    public GameObject completeDisplayObject;
    public AudioClip completeAudioClip;
    public DialogDisplay.DialogStruct[] incompletePrompt;
    public DialogDisplay.DialogStruct completePrompt;
    public DialogDisplay.DialogStruct[] alreadyCompletedPrompt;

    private static int[,] currentGrid = new int[3, 3];
    private static bool opened = false;

    public static bool Complete = false;

    protected override void StartUIObject()
    {
        base.StartUIObject();
        if (Complete)
        {
            SetupSolved();
        }
        else
        {
            SetupUnsolved();
        }
        ItemsChanged();
        opened = true;
    }

    protected override void ItemsChanged()
    {
        base.ItemsChanged();
        for(int i=0; i<snapColliders.Length; i++)
        {
            int t = 0;
            if (snapedTiles[i].Count > 0)
            {
                t = snapedTiles[i][0]+1;
            }
            currentGrid[i / 3, i % 3] = t;
        }
        SumIt();
    }

    private void SetupSolved(bool onBegin = true)
    {
        ForceSnap(0, 5, true);
        ForceSnap(1, 0, true);
        ForceSnap(2, 7, true);
        ForceSnap(3, 6, true);
        ForceSnap(4, 4, true);
        ForceSnap(5, 2, true);
        ForceSnap(6, 1, true);
        ForceSnap(7, 8, true);
        ForceSnap(8, 3, true);
        if (onBegin) DialogDisplay.NewDialog(alreadyCompletedPrompt);
        else DialogDisplay.NewDialog(completePrompt);
    }

    private void SetupUnsolved()
    {
        //ForceSnap(0, 5, true);
        //ForceSnap(5, 2, true);
        //ForceSnap(7, 8, true);
        for (int i = 0; i < snapColliders.Length; i++)
        {
            int t = currentGrid[i / 3, i % 3]-1;
            if(t >= 0)
            {
                ForceSnap(t, i, false);
            }
        }
        if(!opened)DialogDisplay.NewDialog(incompletePrompt);
    }

    private void SumIt()
    {
        int[] sums = new int[8];
        sums[0] = currentGrid[0, 0] + currentGrid[1, 0] + currentGrid[2, 0];
        sums[1] = currentGrid[0, 1] + currentGrid[1, 1] + currentGrid[2, 1];
        sums[2] = currentGrid[0, 2] + currentGrid[1, 2] + currentGrid[2, 2];

        sums[3] = currentGrid[0, 2] + currentGrid[1, 1] + currentGrid[2, 0];

        sums[4] = currentGrid[0, 0] + currentGrid[0, 1] + currentGrid[0, 2];
        sums[5] = currentGrid[1, 0] + currentGrid[1, 1] + currentGrid[1, 2];
        sums[6] = currentGrid[2, 0] + currentGrid[2, 1] + currentGrid[2, 2];

        sums[7] = currentGrid[0, 0] + currentGrid[1, 1] + currentGrid[2, 2];

        bool complete = true;
        for(int i = 0; i < 8; i++)
        {

            string s = sums[i].ToString("D2");
            if (sums[i] == 15)
            {
                complete &= true;
                s = "<color=green>" + s + "</color>";
            }
            else
            {
                complete = false;
                s = "<color=red>" + s + "</color>";
            }
            texts[i].text = s;
        }

        if (complete)
        {
            Complete = true;
            SetupSolved(false);
            InstantiateUIElement(completeDisplayObject);
            AudioHandler.Instance.playSoundEffect(completeAudioClip);
        }

        
    }


}
