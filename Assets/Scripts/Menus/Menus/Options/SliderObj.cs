using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SliderObj : MonoBehaviour {

	[SerializeField] private Slider SliderToReference;
	[SerializeField] private AudioMixerGroup GroupToChange;
	[SerializeField] private float Test;
	[SerializeField] private float Divisor;
	private float XPos;
	void Start () {
		XPos = transform.position.x;
		Test = SliderToReference.value;
		UpdateGroup ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (XPos + (SliderToReference.value / Divisor), transform.position.y, transform.position.z);
		Test = SliderToReference.value;
	}

	public void UpdateGroup(){
			GroupToChange.audioMixer.SetFloat (GroupToChange.name, Test);
	}
}
