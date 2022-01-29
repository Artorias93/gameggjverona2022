using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

[System.Serializable]
public struct Sound
{
    public string name;
    public SoundData data;
}

[System.Serializable]
public struct SoundData
{
    public string eventString;
    [HideInInspector] public FMOD.Studio.EventInstance eventInstance;
}


/*
 * USAGE EXAMPLE: AudioManager.Play("Jump");
 */

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance; //singleton

    public Sound[] sounds;
    public Dictionary<string, SoundData> soundsDict = new Dictionary<string, SoundData>();

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].data.eventInstance = FMODUnity.RuntimeManager.CreateInstance(sounds[i].data.eventString);
            soundsDict.Add(sounds[i].name, sounds[i].data);
        }
    }

    public static void Play(string soundName)
    {
        EventInstance eventInstance;
        PLAYBACK_STATE state;

        instance.soundsDict[soundName].eventInstance.getPlaybackState(out state);

        /**
         * If the sound is not playing, just play it.
         * Otherwise, create a new instance of the sound and play the instance instead.
         * This is for not stopping the sound if it is already playing
         * -> memory hungry if the sound gets played often in a small amount of time
         */
        if (state != PLAYBACK_STATE.PLAYING)
        {
            eventInstance = instance.soundsDict[soundName].eventInstance;
        }
        else
        {
            eventInstance = FMODUnity.RuntimeManager.CreateInstance(instance.soundsDict[soundName].eventString);
        }
            
        eventInstance.start();
    }


    /* To change game music from menu to level: 
        AudioManager.SetParameterByName("Music", "GAME STATE", 1);
    */
    public static void SetParameterByName(string soundName, string parameterName, float value)
    {
        instance.soundsDict[soundName].eventInstance.setParameterByName(parameterName, value);
    }
}
