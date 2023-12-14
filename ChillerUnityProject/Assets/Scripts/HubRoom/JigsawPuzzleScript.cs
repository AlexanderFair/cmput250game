using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the puzzle ui object
public class JigsawPuzzleScript : SnapManagerUIObject
{

    [Header("Jigsaw Puzzle Settings")]
    //Each element in the stucts must be contained by the respective snap object
    public JigsawSolutionElement[] solution;
    //The sound effect to be played when the solution is complete
    public AudioClip completeSound;
    //The sound effect to be played when the solution is incorrect
    public AudioClip incorrectSound;
    //The object to instantiate when the solution is correct
    public GameObject completeObject;
    //The object to instantiate when the solution is incorrect
    public GameObject incorrectObject;

    private JigsawRoomObject room;

    public void Setup(JigsawRoomObject _room)
    {
        room = _room;
        if (room.IsSolved())
        {
            foreach (JigsawSolutionElement sol in solution)
            {
                dragableTiles[sol.elementIndex].Snap(snapColliders[sol.colliderIndex]);
            }
            InstantiateUIElement(completeObject);
        }
    }

    //Called when an object is released
    protected override void ItemsChanged()
    {
        base.ItemsChanged();
        foreach (JigsawSolutionElement _solution in solution)
        {
            if (snappedObjects[_solution.elementIndex] != _solution.colliderIndex)
            {
                UpdateIncomplete();
                return;
            }
        }
        UpdateComplete();
    }

    // Called by UpdateSnaps if the puzzle is complete
    protected virtual void UpdateComplete()
    {
        AudioHandler.Instance.playSoundEffect(completeSound);
        if (completeObject != null) { InstantiateUIElement(completeObject); }
        room.SetSolved();
    }

    //Called by UpdateSnaps if the puzzle is still incomplete
    protected virtual void UpdateIncomplete()
    {
        AudioHandler.Instance.playSoundEffect(incorrectSound);
        if(incorrectObject != null) { InstantiateUIElement(incorrectObject); }
    }

    public bool IsPuzzleComplete()
    {
        return room.IsSolved();
    }
}


[System.Serializable]
public struct JigsawSolutionElement
{
    //The indexsd of the collider the element should be snapped to in the collider list
    public int colliderIndex;
    //the index of the snapable that should be snapped to the collider in the dragable list
    public int elementIndex;
}