using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<Sound> sounds;
    public static AudioManager instance;

    public const string BGM = "BGM";
    public const string CLICK_SFX = "ButtonClick";
    public const string BALL_KICK_SFX = "BallKick";
    public const string APPLAUSE_SFX = "Applause";
    public const string HORN_SFX = "Horn";
    public const string GOAL_SFX = "Goal";
    public const string PUNCH_SFX = "Punch";

    private void Awake()
    {

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
        }
    }

    private void Start()
    {
        PlayAudio(BGM);
    }

    public void PlayAudio(string name)
    {
        Sound sound = sounds.Find(s => s.name == name);
        if(sound == null)
        {
            return;
        }

        sound.source.Play();

    }

    public float GetAudioDuration(string name)
    {
        Sound sound = sounds.Find(s => s.name == name);
        if (sound == null)
        {
            return 0;
        }

        return sound.clip.length;
    }

}
