using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Options : MonoBehaviour {

	[SerializeField] private Slider SliderSelected;

	//opção + de volume
	public void Plus(){
		if(SliderSelected.value < SliderSelected.maxValue)
			SliderSelected.value += Time.deltaTime;
	}

	//opção - de volume
	public void Minus(){
		if (SliderSelected.value > SliderSelected.minValue)
			SliderSelected.value -= Time.deltaTime;
	}
}
