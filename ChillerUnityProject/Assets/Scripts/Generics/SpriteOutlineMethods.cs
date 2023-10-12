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
    /*
     * Must be explicitly called when the outline existance should be update (on the update call)
     */
    public static void UpdateOutlinableSprite<T>(this T outlineableSprite, SpriteRenderer clickableSpriteRenderer) where T : MonoBehaviour, IOutlineableSprite
    {
        if (clickableSpriteRenderer==null)
        {
            Settings.DisplayWarning("Does not have a spriteRenderer attached but is trying to outline", outlineableSprite.gameObject);
            return;
        }

        Material m = Settings.PrefabMaterials.Null.Get();

        if(outlineableSprite is IDragableSprite drag)
        {
            if (drag.ClickableCondition()) m = Settings.PrefabMaterials.Dragable.Get();
        }
        else if(outlineableSprite is IClickableSprite click)
        {
            if (click.ClickableCondition()) m = Settings.PrefabMaterials.Clickable.Get();
        }
        else if(outlineableSprite is IInteractableSprite inter)
        {
            if (inter.InteractableCondition()) m = Settings.PrefabMaterials.Interactable.Get();
        }

        clickableSpriteRenderer.material = m;
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


