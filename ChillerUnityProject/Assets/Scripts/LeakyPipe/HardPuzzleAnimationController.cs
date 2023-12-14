using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardPuzzleAnimationController : MonoBehaviour
{
    public AnimationSpriteClass animator;
    public Sprite[] incompleteAnimation;
    public Sprite[] completeAnimation;

    private bool complete = false;


    private void Start()
    {
        CheckComplete();
    }

    private void Update()
    {
        CheckComplete();
    }

    private void CheckComplete()
    {
        if (PipeGrid.getPuzzle(PipeGrid.PipePuzzles.HARD).isSolved())
        {
            if (!complete)
            {
                Debug.Log("change");
                animator.ChangeAnimation(completeAnimation);
            }
            complete = true;
        }
        else
        {
            if (complete)
            {
                Debug.Log("change");
                animator.ChangeAnimation(incompleteAnimation);
            }
            complete = false;
        }
    }
}
