using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the room object
public class HubJigsawRoomObject : JigsawRoomObject
{
    [Header("Puzzle Settings")]
    public Sprite incompleteSprite;
    public Sprite completeSprite;

    public static bool Complete { get; private set; } = false;

    public override void Start()
    {
        base.Start();
        interactableRenderer.sprite = Complete ? completeSprite : incompleteSprite;
    }

    public override void SetSolved()
    {
        Complete = true;
        interactableRenderer.sprite = completeSprite;
    }

    public override bool IsSolved()
    {
        return Complete;
    }
}
