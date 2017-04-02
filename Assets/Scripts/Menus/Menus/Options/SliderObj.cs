using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderObj : MonoBehaviour {

	[SerializeField] private Slider SliderToReference;
	private float XPos;
	void Start () {
		XPos = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		//mantem o objeto na posição de acordo com Slider.
		transform.position = new Vector3 (XPos + SliderToReference.value, transform.position.y, transform.position.z);
	}
}
