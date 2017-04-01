using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderObj : MonoBehaviour {

	[SerializeField] private Slider SliderToReference;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (SliderToReference.value, transform.position.y, transform.position.z);
	}
}
