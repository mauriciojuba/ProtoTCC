using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFX : MonoBehaviour {

    public AudioMixerGroup Mixer;
	[SerializeField] private AudioSource Audio;

    public void PlaySoundSFX(string Name)
    {
        SoundManager.PlaySFX(gameObject, Name);
		Audio = GetComponent<AudioSource> ();
		Audio.outputAudioMixerGroup = Mixer;
    }
}
