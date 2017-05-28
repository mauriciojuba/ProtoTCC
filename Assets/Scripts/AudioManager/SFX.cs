﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFX : MonoBehaviour {


    public string Objeto;
    public AudioMixerGroup Mixer;
	[SerializeField] private AudioSource Audio;

    public void PlaySoundSFX(string Name)
    {
        SoundManager.PlaySFX(gameObject, Name);
		Audio = GetComponent<AudioSource> ();
		Audio.outputAudioMixerGroup = Mixer;
    }

    public void AplicaMixer()
    {
        Audio = GetComponent<AudioSource>();
        Audio.outputAudioMixerGroup = Mixer;
    }

    void OnCollisionEnter(Collision hit)
    {
        if(hit.gameObject.name == "Chao")
        {
            if(Objeto == "Vidro")
            {
                SoundManager.PlaySFX(gameObject,SoundManager.LoadFromGroup("Objetos quebrando vidro"));
                Audio = GetComponent<AudioSource>();
                Audio.outputAudioMixerGroup = Mixer;
            }



        }
    }
}

