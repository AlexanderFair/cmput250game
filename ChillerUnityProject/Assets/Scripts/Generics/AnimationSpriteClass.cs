using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A class for animations on the given sprite render
 * 
 * This class does not update by itself and its UpdateAnimation method
 * must be explicitly called from another update method.
 * 
 * To use this on an object, attach the script to the obect and call the update method
 * from that scripts update method
 */
public class AnimationSpriteClass : MonoBehaviour
{
    
    // The nothing animation -- no sprites are rendered
    public static readonly Sprite[] NULL_STRUCT = { null };
    [Header("Animation Sprite Settings")]
    // The animation struct to be rendered
    public Sprite[] animationStruct;
    public SpriteRenderer spriteRenderer;
    // The amount of frames that each give frame should represet
    // if this is 2 then each frame will be shown twice consecutively.
    // i.e. it is now AABBCC instead of ABC
    public float repetitionFactor = 1;
    private bool hasSoundsOnAnimation = false;
    private bool[] playSoundOnThisFrame = {false};
    private AudioClip[] sounds = new AudioClip[0];

    protected float currentTime = 0;
    protected int currentFrame = 0;
    protected bool activeAnimation = true;

    // The current time elapsed on the current frame
    public float CurrentFrameTime { get { return currentTime; } }
    // The current frame of the animation
    public int CurrentFrame { get { return currentFrame; } }
    // If the animation is running
    public bool IsAnimationActive { get { return activeAnimation; } }

    // Start is called before the first frame update
    public void AwakeAnimation()
    {
        spriteRenderer.sprite = animationStruct[0];
        if(repetitionFactor < 1)
        {
            throw new System.ArgumentOutOfRangeException("The repetition factor can not be less than 1 in " + name);
        }
    }

    // Update is called once per frame
    public void UpdateAnimation()
    {
        currentTime += Time.deltaTime / repetitionFactor;
        if ( currentTime >= 1f/Settings.FloatValues.FPS.Get())
        {
            currentFrame++;
            currentFrame %= animationStruct.Length;
            currentTime -= 1f / Settings.FloatValues.FPS.Get();
            if (hasSoundsOnAnimation && playSoundOnThisFrame[currentFrame]){
                AudioHandler.Instance.playSoundEffect(Util.ChooseRandom(sounds));
            }
        }

        SetRender(animationStruct[currentFrame]);
    }

    // Sets the currently rendering sprite
    private void SetRender(Sprite s)
    {
        if ( s != null )
        {
            spriteRenderer.enabled = true;
            spriteRenderer.sprite = s;
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }

    /*
     * If the animation is paused, 
     * this will cause the animation to play from where ever it was paused at
     */
    public void StartAnimation()
    {
        activeAnimation = true;
    }

    /*
     * Pauses the animation at the current frame
     */
    public void PauseAnimation()
    {
        activeAnimation = false;
    }

    /*
     * Starts the animation from the beginning
     */
    public void RestartAnimation()
    {
        ResetAnimation();
        StartAnimation();
    }

    /*
     * Changes the current animation to the newAnimation.
     * 
     * If restart is flagged, the animation is started from the first frame
     * Otherwise, the animation will start (or be paused) at the same time/frame 
     * as the previous animation was last at
     */
    public void ChangeAnimation(Sprite[] newAnimation, bool restart = true, bool[] newPlaySoundPerFrame = null, AudioClip[] newSounds = null)
    {
        if (newSounds == null && newPlaySoundPerFrame == null){
            sounds = new AudioClip[0];
            playSoundOnThisFrame = new bool[0];
            hasSoundsOnAnimation = false;
        } else if (newSounds != null && newPlaySoundPerFrame != null) {
            if (newPlaySoundPerFrame.Length != newAnimation.Length)
                throw new System.Exception("Number of frames and number of playSoundPerFrame doesn't match!");
            sounds = newSounds;
            hasSoundsOnAnimation = true;
            playSoundOnThisFrame = newPlaySoundPerFrame;
        } else {
            throw new System.Exception("Exactly one of newPlaySoundPerFrame and newSounds were null which is not allowed!");
        }
        
        animationStruct = newAnimation;
        if(restart)
        {
            RestartAnimation();
        }
    }

    /*
     * Resets the current frame in the animation to the beginning
     * 
     * This does not change if the animation is paused or not
     */
    public void ResetAnimation()
    {
        currentTime = 0;
        currentFrame = 0;
    }

}
