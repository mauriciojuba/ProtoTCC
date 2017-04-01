using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Options : MonoBehaviour {

	[SerializeField] private Slider SliderSelected;
	[SerializeField] private GameObject Lever, OptionObj;

	[SerializeField] private float Speed;
	[Range(1.5f,5)]
	[SerializeField] private float SliderSpeed;

	private float InitRot , AngleToGo, PosToGo, InitPos;

	float rotX;
	float rotY;
	float rotZ;

	float Direction;
	void Start(){
		SetInitialRot ();
	}

	void Update(){
		if (Lever != null) {
			Direction = Input.GetAxis ("Horizontal P1");
			AngleToGo = Mathf.MoveTowards (AngleToGo, Direction, Time.deltaTime * Speed);
			PosToGo = Mathf.MoveTowards (PosToGo, Direction, Time.deltaTime * Speed);

			Lever.transform.localEulerAngles = new Vector3 (rotX, rotY, InitRot + (AngleToGo * 45f * -1));
			if (Direction > 0.2f) {
				Plus ();
			} else if (Direction < 0.2f) {
				Minus ();
			}
		}
	}


	public void SelectLever(GameObject lever, GameObject optionObj){
		Lever = lever;
		OptionObj = optionObj;
	}

	public void DeselectLever(){
		Lever = null;
		OptionObj = null;
	}

	void SetInitialRot(){
		InitRot = Lever.transform.localEulerAngles.z;
		InitPos = OptionObj.transform.position.x;

		rotX = Lever.transform.localEulerAngles.x;
		rotY = Lever.transform.localEulerAngles.y;
		rotZ = Lever.transform.localEulerAngles.z;
	}

	//opção + de volume
	public void Plus(){
		if(SliderSelected.value < SliderSelected.maxValue)
			SliderSelected.value += Time.deltaTime * Mathf.Abs(Direction) * SliderSpeed;

//		OptionObj.transform.position = new Vector3 (InitPos + (PosToGo * 7.5f * -1),
//			OptionObj.transform.position.y,
//			OptionObj.transform.position.z);
	}

	//opção - de volume
	public void Minus(){
		if (SliderSelected.value > SliderSelected.minValue)
			SliderSelected.value -= Time.deltaTime * Mathf.Abs(Direction) * SliderSpeed;

//		OptionObj.transform.position = new Vector3 (InitPos + (PosToGo * 7.5f * -1),
//			OptionObj.transform.position.y,
//			OptionObj.transform.position.z);
	}
}
