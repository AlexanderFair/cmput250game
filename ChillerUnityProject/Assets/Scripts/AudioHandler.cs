using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Class that handles audio, both sound effects and sountrack (soundtrack yet to be implemented)
 * 
 * Usage: Add an empty game object to the scene (name it AudioHandler for consistency)
 * and add this script to the object. Interactable UI room objects have this by default
 * for the interact sound. 
*/
public class AudioHandler : MonoBehaviour
{
    /* Singleton */
    private static AudioHandler _instance;
    public static AudioHandler Instance { 
        get {
            if ((object)_instance == null){
                throw new System.NullReferenceException("AudioHandler does not exist. Most likely forgot to add AudioHandler Obj/script to the scene.");
            }
            return _instance;
        }
    }

    private AudioSource soundtrackAudioSource;

    /* The soundtracks for the game */
    public AudioClip[] tracks;
    
    /* The insanity scores for each track. These determine which tracks play at
     * which tim */
    public int[] trackInsanityScores;

    /* The audio source (not the clip!) for sound effects. 
    * Unsure if this will stay seperate from the soundtrack*/
    private AudioSource effectSource;

    void Awake(){
        bool shouldDestroy = true;
        try {
            if ((object)Instance != null && Instance != this) {
                shouldDestroy = true;
            }
        }
        catch {
            shouldDestroy = false;
        }

        if (shouldDestroy){
            Settings.DisplayWarning("Stopped creation of extra audio handler!!", this.gameObject);
            Destroy(this);
        } else {
            _instance = this;
        }

        if (tracks.Length != trackInsanityScores.Length){
            throw new System.Exception("Each track must have a corresponding trackInsanityScore in the AudioHandler");
        }

        this.soundtrackAudioSource = gameObject.AddComponent<AudioSource>();
        soundtrackAudioSource.volume = 0.1f;
        this.effectSource = gameObject.AddComponent<AudioSource>();
        DontDestroyOnLoad(this);
    }

    void Update(){
        if (!soundtrackAudioSource.isPlaying){
            AudioClip nextTrack = chooseSoundtrack();
            //dont play if theres nothing to play
            if ((object)nextTrack != null){
                soundtrackAudioSource.clip = nextTrack;
                soundtrackAudioSource.Play();
            }
        }
    }
    //TEMP THING
    public bool doplay = false;
    /* Chooses a track based off of insanity/game progress */
    private AudioClip chooseSoundtrack(){
        if (doplay){
            return tracks[0];
        }
        return null;
    }
    /* Plays a sound effect. 
    * @param AudioClip soundEffect The sound effect to be played
    */
    public void playSoundEffect(AudioClip soundEffect){
        if(soundEffect == null)
        {
            Settings.DisplayWarning("The sound effect is null", null);
            return;
        }
        this.effectSource.PlayOneShot(soundEffect);
    }
}