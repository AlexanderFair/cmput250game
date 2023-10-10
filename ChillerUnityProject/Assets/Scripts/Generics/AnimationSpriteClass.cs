using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSpriteClass : MonoBehaviour
{
    // The nothing animation -- no sprites are rendered
    public static readonly AnimationStruct NULL_STRUCT = new AnimationStruct(new List<Sprite>() { null }, 0f);

    // The animation struct to be rendered
    public AnimationStruct animationStruct = NULL_STRUCT;
    public SpriteRenderer spriteRenderer;

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
    void Start()
    {
        spriteRenderer.sprite = animationStruct.sprites[0];
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if ( currentTime >= animationStruct.Spf)
        {
            currentFrame++;
            currentFrame %= animationStruct.Frames;
            currentTime -= animationStruct.Spf;
        }

        SetRender(animationStruct.sprites[currentFrame]);
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
    public void ChangeAnimation(AnimationStruct newAnimation, bool restart = true)
    {
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

    /*
     * A structure used to store information about an animation
     */
    [System.Serializable]
    public struct AnimationStruct
    {   
        //The list of sprites
        public List<Sprite> sprites;
        // the FPS of the animation
        public float fps;

        // Takes _sprites - the list of sprites, and _fps - the fps of the anim
        public AnimationStruct(List<Sprite> _sprites, float _fps) 
        {
            sprites = _sprites;
            fps = _fps;
        }

        // The seconds per frame of the animation
        public float Spf { get { return 1f / fps; } }
        // The number of frames in the 
        public int Frames { get { return sprites.Count; } }
    }
}
