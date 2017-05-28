using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFX : MonoBehaviour {

    public AudioMixerGroup Mixer;

    public void PlaySoundSFX(string Name)
    {
        SoundManager.PlaySFX(Name);
        SoundManager.GetCurrentAudioSource().outputAudioMixerGroup = Mixer;
    }
}
