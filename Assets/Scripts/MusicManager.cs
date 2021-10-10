using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MusicManager : MonoBehaviour
{
    public class AudioBuffer
    {
        public string name;
        public float time;
    }

    static MusicManager s_Instance;
    public static MusicManager Instance => s_Instance;

    public AudioSource bgmAudioSource;
    public AudioSource sfxAudioSource;
    public AudioClip bgmClip;
    public AudioClip thumbsUpClip;
    public AudioClip thumbsDownClip;
    public AudioClip raiseHandClip;

    public float bufferTime;
    public List<AudioBuffer> audioBuffers = new List<AudioBuffer>();

    void Awake()
    {
        if (s_Instance != null)
        {
            Destroy(this);
            return;
        }

        s_Instance = this;
    }

    void Start()
    {
        bgmAudioSource.clip = bgmClip;
        bgmAudioSource.Play();
    }

    void Update()
    {
        if (audioBuffers.Capacity > 0)
        {
            foreach (AudioBuffer b in audioBuffers)
            {
                b.time -= Time.deltaTime;
            }
        }
    }

    void PlayThumbsUpClip()
    {
        PlayClip(thumbsUpClip);
    }    
    
    void PlayThumbsDownClip()
    {
        PlayClip(thumbsDownClip);
    }    
    
    void PlayRaiseHandClip()
    {
        PlayClip(raiseHandClip);
    }

    public void PlayClip(AudioClip clip)
    {
        if(clip == null)
        {
            Debug.Log("Null Clip");
            return;
        }

        if (audioBuffers.Capacity > 0)
        {
            foreach (AudioBuffer b in audioBuffers)
            {
                if (clip.name == b.name && b.time > 0)
                {
                    return;
                }
                else if (clip.name == b.name)
                {
                    b.time = bufferTime;
                }
            }
        }

        sfxAudioSource.PlayOneShot(clip);

        AudioBuffer buffer = new AudioBuffer();
        buffer.name = clip.name;
        buffer.time = bufferTime;
        audioBuffers.Add(buffer);
    }
}

