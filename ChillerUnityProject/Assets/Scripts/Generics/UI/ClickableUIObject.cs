using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A subclass for ui objects which can be interacted with by clicking
 */
public abstract class ClickableUIObject : UIObjectClass, IClickableSprite
{
    [Header("Clickable UI Settings")]
    // The collider for the object where the mouse can click on
    public Collider2D clickableCollider;

    // The sprite renderer which should obtain an outline when the sprite can be clicked
    public SpriteRenderer spriteClickableOutlineRenderer;
    //The indexs of animator to use for updating sprites within the sprite animators list -- if blank uses the spriteClickableOutlineRenderer
    //-1 means no animator
    public int animatorIndex = -1;
    //Animation to display if the button can be clicked, if there is not an animation class, the 0th sprite is used
    public Sprite[] clickableAnimation;
    //Animation to display if the button cannot be clicked, if there is not an animation class, the 0th sprite is used
    public Sprite[] clickedAnimation;
    //Time to wait between clicks
    public float clickRechargeTime;
    //The sound effect to be played when clicked
    public AudioClip[] soundEffect = new AudioClip[0];

    public Settings.Controls clickControl = Settings.Controls.Click;

    private bool clicked = false;
    private float timer = 0;

    protected override void AwakeUIObject()
    {
        base.AwakeUIObject();
        if (spriteClickableOutlineRenderer == null && animatorIndex < 0)
        {
            Settings.DisplayError("Both the renderer and animator are blank", gameObject);
            return;
        }
        if (clickableAnimation.Length == 0)
        {
            clickableAnimation = new Sprite[] { spriteClickableOutlineRenderer.sprite };
        }
        if(clickedAnimation.Length == 0)
        {
            clickedAnimation = new Sprite[] { clickableAnimation[0] };
        }
        ChangeAnim();
        this.AwakeOutlinableSprite(spriteClickableOutlineRenderer);
    }
    protected override void UpdateUIObject()
    {
        this.UpdateOutlinableSprite();
        bool shouldChange = false;
        if(Util.GetKeyDownWithMouseOverObject(clickControl, clickableCollider) 
            && ClickableCondition())
        {
            Clicked();
            clicked = true;
            timer = 0;
            shouldChange = true;
        }
        if (clicked) 
        {
            timer += Time.deltaTime;
            if(timer >= clickRechargeTime)
            {
                timer = 0;
                clicked = false;
                shouldChange = true;
            }
        }

        if (!ClickableCondition() && !clicked)
        {
            clicked = true;
            timer = clickRechargeTime;
            shouldChange = true;
        }

        if (shouldChange)
        {
            ChangeAnim();
        }
    }
    //Called when the animation should be changed to a new animation
    protected virtual void ChangeAnim()
    {
        UpdateRendererWithNewSprite(clicked ? clickedAnimation : clickableAnimation);
    }

    //Called after by ChangeSprite with the animation to use.
    // Makes the sprite render use the first sprite if an animation is not present otherwise continues the animation with the new sprite
    protected virtual void UpdateRendererWithNewSprite(Sprite[] anim)
    {
        if(animatorIndex < 0)
        {
            spriteClickableOutlineRenderer.sprite = anim[0];
        }
        else
        {
            spriteAnimators[animatorIndex].ChangeAnimation(anim, false);
        }
    }

    /*
     * The condition for the object to be interactable.
     * Defaults to always if this method is not overriden.
     */
    public virtual bool ClickableCondition() { return !clicked; }
    /*
     * Called when the object is interacted with
     */
    protected virtual void Clicked() {
        if(soundEffect.Length > 0)
        {
            AudioHandler.Instance.playSoundEffect(Util.ChooseRandom(soundEffect));
        }
        clickControl.UseControl();
    }
}
