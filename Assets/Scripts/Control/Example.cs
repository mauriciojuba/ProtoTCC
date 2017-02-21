using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example : MonoBehaviour {

	public string[] Joysticks;

	void Update() {
		Joysticks = Input.GetJoystickNames ();
	}
}
