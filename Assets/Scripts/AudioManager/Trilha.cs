using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Trilha : MonoBehaviour {

    public AudioMixerGroup Mixer;
	[SerializeField] private AudioSource[] Audio;
	void Start(){
		Audio = new AudioSource[2];
	}

    public void Update()
    {

		for (int i = 0; i < Audio.Length; i++) {
			if (Audio[i] == null) {
				Audio = GetComponents<AudioSource> ();
			}


			if (Audio [i].outputAudioMixerGroup != Mixer) {
				Audio [i].outputAudioMixerGroup = Mixer;
			}
		}
    }
}
