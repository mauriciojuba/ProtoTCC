using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SliderObj : MonoBehaviour {

	[SerializeField] private Slider SliderToReference;
	[SerializeField] private AudioMixerGroup GroupToChange;
	[SerializeField] private float Test;
	private float XPos;
	void Start () {
		XPos = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (XPos + SliderToReference.value, transform.position.y, transform.position.z);
		//pegar o valor do slider aqui
	}

	public void UpdateGroup(){
		GroupToChange.audioMixer.SetFloat (GroupToChange.name, SliderToReference.value);
	}
}
