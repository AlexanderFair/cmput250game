using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

/* A static class of methods for outlinable sprites
 * 
 * Inherit one of the interfaces below to gain access to an outline
 */
public static class SpriteOutlineMethods
{
    public static void AwakeOutlinableSprite<T>(this T outlineableSprite, SpriteRenderer outlineableSpriteRenderer) where T : MonoBehaviour, IOutlineableSprite
    {
        if (outlineableSpriteRenderer == null)
        {
            Settings.DisplayWarning("Does not have a spriteRenderer attached but is trying to outline", outlineableSprite.gameObject);
            return;
        }

        OutlineSpriteClass outliner = outlineableSprite.gameObject.GetComponent<OutlineSpriteClass>() ?? outlineableSprite.gameObject.AddComponent<OutlineSpriteClass>();
        outliner.SetupOutline(outlineableSpriteRenderer, outlineableSprite.OutlineType().Get());

    }
    /*
     * Must be explicitly called when the outline existance should be update (on the update call)
     */
    public static void UpdateOutlinableSprite<T>(this T outlineableSprite) where T : MonoBehaviour, IOutlineableSprite
    {
        OutlineSpriteClass outliner = outlineableSprite.gameObject.GetComponent<OutlineSpriteClass>();
        if(outliner == null)
        {
            Settings.DisplayWarning("Outliner is null", outlineableSprite.gameObject);
            return;
        }
        if (outlineableSprite.OutlineCondition())
        {
            outliner.TurnOn();
        }
        else
        {
            outliner.TurnOff();
        }
        outliner.UpdateOutliner();

    }

    public static bool OutlineCondition(this IOutlineableSprite outlineableSprite)
    {
        switch (outlineableSprite)
        {
            case IDragableSprite d : return d.ClickableCondition();
            case IClickableSprite c: return c.ClickableCondition();
            case IInteractableSprite i: return i.InteractableCondition();
            case ICollisionInteractionSprite col: return col.CollisionInteractionCondition();
            default: return false;
        }
    }

    public static Settings.Outlines OutlineType(this IOutlineableSprite outlineableSprite)
    {
        switch (outlineableSprite)
        {
            case IDragableSprite _: return Settings.Outlines.Drag;
            case IClickableSprite _: return Settings.Outlines.Click;
            case IInteractableSprite _: return Settings.Outlines.Interact;
            case ICollisionInteractionSprite _: return Settings.Outlines.Collision;
            default: return Settings.Outlines.Null;
        }
    }

   
}

public interface IOutlineableSprite
{
}
public interface IClickableSprite : IOutlineableSprite
{
    bool ClickableCondition();
}

public interface IDragableSprite : IClickableSprite
{

}

public interface IInteractableSprite : IOutlineableSprite
{
    bool InteractableCondition();
}

public interface ICollisionInteractionSprite : IOutlineableSprite
{
    bool CollisionInteractionCondition();
}


