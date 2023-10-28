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
public class AudioHandler : MonoBehaviour, Settings.ISettingsUpdateWatcher
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
    public float[] trackInsanityScores;
    
    /* The volume for the soundtrack */
    //public float soundTrackVolume = 1f;
    /* The volume for ambient noise */
    //public float ambientVolume = 1f; ---These settings are now located in Settings

    /* The audio source (not the clip!) for sound effects. 
    * Unsure if this will stay seperate from the soundtrack*/
    private AudioSource effectSource;
    /* Audio source for wind and other ambient sounds */
    private AudioSource ambientSource;
    /* The wind noise that is always looping */
    public AudioClip windNoiseLoop = null;

    [Header("Volume Settings")]
    public float trackVolume = 1;
    public float effectVolume = 1;
    public float ambientVolume = 1;

    /* How long between tracks */
    private float cooldown = 10f;
    /* If a track just finished playing - dont play another right away. Determines if its on cooldown */
    private float cooldownTimer = 0f;

    // only play if this is zero, things will ask it to pause by incrementing this counter
    private int ambientPause = 0;

    // only play if this is zero, things will ask it to pause by incrementing this counter
    private int soundtrackPause = 0;

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
            DestroyImmediate(gameObject);
            return;
        } else {
            _instance = this;
        }

        if (tracks.Length != trackInsanityScores.Length){
            throw new System.Exception("Each track must have a corresponding trackInsanityScore in the AudioHandler");
        }

        this.soundtrackAudioSource = gameObject.AddComponent<AudioSource>();
        this.effectSource = gameObject.AddComponent<AudioSource>();
        this.ambientSource = gameObject.AddComponent<AudioSource>();

        SetVolumes();

        DontDestroyOnLoad(this);

        ambientSource.loop = true;
        if ((object)windNoiseLoop == null){
            throw new System.Exception("Wind noise is not set in Audio Handler");
        }
        ambientSource.clip = windNoiseLoop;
        ambientSource.Play();
        this.AwakeSettingsWatcher();

    }

    private void SetVolumes()
    {
        soundtrackAudioSource.volume = Settings.FloatValues.SoundtrackVolume.Get() * Settings.FloatValues.MasterVolume.Get() * trackVolume;
        ambientSource.volume = Settings.FloatValues.AmbientVolume.Get() * Settings.FloatValues.MasterVolume.Get() * ambientVolume;
        effectSource.volume = Settings.FloatValues.SoundEffectVolume.Get() * Settings.FloatValues.MasterVolume.Get() * effectVolume;
    }

    void Update(){
        // handle pausing
        if(soundtrackAudioSource.isPlaying && soundtrackPause > 0){
            soundtrackAudioSource.Pause();
        }
        else if (!soundtrackAudioSource.isPlaying && soundtrackPause == 0){
            soundtrackAudioSource.UnPause();
        }

        if (ambientSource.isPlaying && ambientPause > 0){
            ambientSource.Pause();
        } else if (!ambientSource.isPlaying && ambientPause == 0){
            ambientSource.UnPause();
        }

        if (!soundtrackAudioSource.isPlaying && soundtrackPause == 0){
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer > cooldown){
                AudioClip nextTrack = chooseSoundtrack();
                //dont play if theres nothing to play
                if ((object)nextTrack != null){
                    soundtrackAudioSource.clip = nextTrack;
                    soundtrackAudioSource.Play();
                    cooldownTimer = 0;
                }
            }
        }
    }
    //TEMP THING
    public bool doplay = false;
    /* Chooses a track based off of insanity/game progress */
    private AudioClip chooseSoundtrack(){
        if (!doplay){
            return null;
        }
        // find number of 
        int countOfPlayableTracks = 0;
        foreach (float score in trackInsanityScores){
            if (score <= Insanity.Instance.getInsanity()){
                countOfPlayableTracks++;
            }
        }
        int choice = Random.Range(0, countOfPlayableTracks+1);
        int count = 0;
        for (int i = 0; i < trackInsanityScores.Length; i++){
            if (trackInsanityScores[i] <= Insanity.Instance.getInsanity()) {
                count++;
                if (count >= choice){
                    return tracks[i];
                }
            }
        }
        Debug.Log("please let xander know if you saw this thanks (choose soundtrack)");
        return tracks[0];
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

    public void FloatValuesUpdated(Settings.FloatValues floatVal)
    {
        switch (floatVal)
        {
            case Settings.FloatValues.MasterVolume:
            case Settings.FloatValues.AmbientVolume:
            case Settings.FloatValues.SoundtrackVolume:
            case Settings.FloatValues.SoundEffectVolume:
                SetVolumes();
                break;
            default:
                break;
        }
    }

    public void ControlsUpdated(Settings.Controls control)
    {

    }

    public void pauseAmbient(){
        ambientPause++;
    }
    public void unpauseAmbient(){
        ambientPause--;
    }

    public void pauseSoundtrack(){
        soundtrackPause++;
    }
    public void unpauseSoundtrack(){
        soundtrackPause--;
    }

}