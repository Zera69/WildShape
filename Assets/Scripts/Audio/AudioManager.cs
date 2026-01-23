using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] music;
    public Sound[] sfx;
    public AudioSource musicSource;
    public AudioSource sfxSource;


    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(music, x => x.name == name);

        if (s != null)
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
        else
        {
            Debug.Log("sonido no encontrado");
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfx, x => x.name == name);

        if (s != null)
        {
            sfxSource.clip = s.clip;
            sfxSource.Play();
        }
        else
        {
            Debug.Log("sonido no encontrado");
        }
    }

}
