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

    private int[,] currentGrid;

    public static bool Complete = false;

    protected override void AwakeUIObject()
    {
        base.AwakeUIObject();
        currentGrid = new int[3,3];
        if (Complete)
        {
            SetupSolved();
        }
        else
        {
            SetupUnsolved();
        }
        ItemsChanged();
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

    private void SetupSolved()
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
    }

    private void SetupUnsolved()
    {
        ForceSnap(3, 6, true);
        ForceSnap(4, 4, true);
        ForceSnap(5, 2, true);
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
            SetupSolved();
            InstantiateUIElement(completeDisplayObject);
            AudioHandler.Instance.playSoundEffect(completeAudioClip);
        }

        
    }


}
